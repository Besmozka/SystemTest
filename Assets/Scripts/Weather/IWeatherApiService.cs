using R3;

namespace Weather
{
    public interface IWeatherApiService
    {
        public Subject<int> OnTemperatureGet { get; }
        public void SendRequest();
        public void StopService();
    }
}