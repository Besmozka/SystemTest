using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using R3;
using Server;
using UnityEngine;
using Zenject;

namespace Weather
{
    public class WeatherController : IDisposable
    {
        private WeatherData _weatherData;
        private IWeatherView _view;
        private IWeatherModel _weatherModel;
        private IAutoExecuter _autoWeatherRequest;
        private IRequestsController _requestsController;
        
        private INavigationPanel _navigationPanel;
        private WeatherRequest _currentRequest;

        private CancellationToken _ct;
        private IDisposable _requestsSubscription;
        private CompositeDisposable _disposables;

        public WeatherController(WeatherData weatherData, CancellationToken ct,
            [Inject(Id = "AutoWeatherRequest")] IAutoExecuter autoWeatherRequest,
            IWeatherView view, IWeatherModel weatherModel,
            IRequestsController requestsController, INavigationPanel navigationPanel)
        {
            _weatherData = weatherData;
            _ct = ct;
            _autoWeatherRequest = autoWeatherRequest;
            _view = view;
            _weatherModel = weatherModel;
            _requestsController = requestsController;
            _navigationPanel = navigationPanel;
            
            _disposables = new CompositeDisposable();

            Init();
        }

        private void Init()
        {
            _weatherModel.Temperature
                .Select(temp => $"{temp} F")
                .Subscribe(temp => _view.ShowTemperatureText(temp))
                .AddTo(_disposables);

            _navigationPanel.WeatherCommand
                .Subscribe(_ => StartService())
                .AddTo(_disposables);
            
            _navigationPanel.ClickerCommand
                .Subscribe(_ => StopService())
                .AddTo(_disposables);
            _navigationPanel.DogsCommand
                .Subscribe(_ => StopService())
                .AddTo(_disposables);
        }

        private void StartService()
        {
            Debug.Log("Starting weather service");
            
            SendRequest();
            
            _autoWeatherRequest.Execute(_ct).Forget();

            _autoWeatherRequest.OnExecute
                .Subscribe(_ => SendRequest())
                .AddTo(_disposables);
        }

        private void SendRequest()
        {
            _currentRequest = new WeatherRequest(_weatherData.WeatherApiUrl);

            _currentRequest.OnTemperatureReceived
                .Subscribe(temp => _weatherModel.SetTemperature(temp))
                .AddTo(_disposables);
            
            _requestsController.EnqueueRequest(_currentRequest);
        }
        
        private void StopService()
        {
            _requestsController.DequeueRequest(_currentRequest);
        }
        
        public void Dispose()
        {
            _currentRequest?.Dispose();
            _disposables?.Dispose();
        }
    }
}