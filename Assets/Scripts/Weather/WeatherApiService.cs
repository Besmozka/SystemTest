using System;
using Cysharp.Threading.Tasks;
using Dogs;
using R3;
using Server;

namespace Weather
{
    public class WeatherApiService : IWeatherApiService, IDisposable
    {
        private WeatherData _weatherData;
        private IBlockPanel _blockPanel;
        private IRequestsController _requestsController;
        
        private Subject<int> _onTemperatureGet;
        public Subject<int> OnTemperatureGet => _onTemperatureGet;
        
        private WeatherRequest _currentRequest;    
        private CompositeDisposable _disposables;

        public WeatherApiService(WeatherData weatherData, IBlockPanel blockPanel, IRequestsController requestsController)
        {
            _weatherData = weatherData;
            _blockPanel = blockPanel;
            _requestsController = requestsController;
            
            _onTemperatureGet = new Subject<int>();
            _disposables = new CompositeDisposable();
        }

        public void SendRequest()
        {
            _currentRequest = new WeatherRequest(_weatherData.WeatherApiUrl);

            _currentRequest.OnTemperatureReceived
                .Subscribe(temp =>
                {
                    OnTemperatureGet.OnNext(temp);
                    _blockPanel.Unblock();
                })
                .AddTo(_disposables);
            
            _blockPanel.Block();
            
            _requestsController.EnqueueRequest(_currentRequest);
        }

        public void StopService()
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