using System;
using System.Threading;
using DefaultNamespace;
using Dogs;
using R3;
using Server;
using UnityEngine;
using Zenject;

namespace Weather
{
    public class WeatherTabController : IDisposable, ITabController
    {
        private WeatherData _weatherData;
        private IWeatherView _weatherView;
        private IWeatherModel _weatherModel;
        private IAutoExecuter _autoWeatherRequest;
        private IRequestsController _requestsController;
        private IBlockPanel _blockPanel;
        
        private WeatherRequest _currentRequest;

        private CancellationTokenSource _cts;
        private CompositeDisposable _disposables;

        public WeatherTabController(WeatherData weatherData, 
            [Inject(Id = "AutoWeatherRequest")] IAutoExecuter autoWeatherRequest,
            IWeatherView weatherView, IWeatherModel weatherModel,
            IRequestsController requestsController, IBlockPanel blockPanel)
        {
            _weatherData = weatherData;
            _autoWeatherRequest = autoWeatherRequest;
            _weatherView = weatherView;
            _weatherModel = weatherModel;
            _requestsController = requestsController;
            _blockPanel = blockPanel;
            
            _cts = new CancellationTokenSource();
            _disposables = new CompositeDisposable();

            Init();
        }

        private void Init()
        {
            _weatherModel.Temperature
                .Select(temp => $"Сегодня:  {temp} F")
                .Subscribe(temp => _weatherView.ShowTemperatureText(temp))
                .AddTo(_disposables);
        }

        public void StartService()
        {
            Debug.Log("Starting weather service");
            
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            SendRequest();
            
            _autoWeatherRequest.Execute(_cts.Token).Forget();

            _autoWeatherRequest.OnExecute
                .Subscribe(_ => SendRequest())
                .AddTo(_disposables);
        }

        private void SendRequest()
        {
            _currentRequest = new WeatherRequest(_weatherData.WeatherApiUrl);

            _currentRequest.OnTemperatureReceived
                .Subscribe(temp =>
                {
                    _weatherModel.SetTemperature(temp);
                    _blockPanel.Unblock();
                })
                .AddTo(_disposables);
            
            _blockPanel.Block();
            
            _requestsController.EnqueueRequest(_currentRequest);
        }
        
        public void StopService()
        {
            _requestsController.DequeueRequest(_currentRequest);
            _cts?.Cancel();
        }

        public void ShowTab()
        {
            _weatherView.SetActive(true);
            
            StartService();
        }

        public void HideTab()
        {
            _weatherView.SetActive(false);
            
            StopService();
        }
        
        public void Dispose()
        {
            _cts?.Dispose();
            _currentRequest?.Dispose();
            _requestsController.Dispose();
            _disposables?.Dispose();
        }
    }
}