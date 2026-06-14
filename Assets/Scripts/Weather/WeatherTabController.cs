using System;
using System.Threading;
using Dogs;
using R3;
using Server;
using UnityEngine;
using Zenject;

namespace Weather
{
    public class WeatherTabController : IDisposable, ITabController
    {
        private IWeatherView _weatherView;
        private IWeatherModel _weatherModel;
        private IWeatherApiService _weatherApiService;
        private IAutoExecuter _autoWeatherRequest;

        private CancellationTokenSource _cts;
        private CompositeDisposable _disposables;

        public WeatherTabController([Inject(Id = "AutoWeatherRequest")] IAutoExecuter autoWeatherRequest,
            IWeatherView weatherView, IWeatherModel weatherModel, IWeatherApiService weatherApiService)
        {
            _autoWeatherRequest = autoWeatherRequest;
            _weatherView = weatherView;
            _weatherModel = weatherModel;
            _weatherApiService = weatherApiService;
            
            _cts = new CancellationTokenSource();
            _disposables = new CompositeDisposable();

            Init();
        }

        private void Init()
        {
            _weatherApiService.OnTemperatureGet
                .Subscribe(_weatherModel.SetTemperature)
                .AddTo(_disposables);
            
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
            
            _weatherApiService.SendRequest();
            
            _autoWeatherRequest.Execute(_cts.Token).Forget();

            _autoWeatherRequest.OnExecute
                .Subscribe(_ => _weatherApiService.SendRequest())
                .AddTo(_disposables);
        }
        
        public void StopService()
        {
            _weatherApiService.StopService();
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
            _disposables?.Dispose();
        }
    }
}