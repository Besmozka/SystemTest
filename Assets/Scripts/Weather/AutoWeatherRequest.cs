using DefaultNamespace;

namespace Weather
{
    public class AutoWeatherRequest : AutoExecuterAsync
    {
        protected override int Delay => _weatherData.AutoRequestTime;
        private WeatherData _weatherData;

        public AutoWeatherRequest(WeatherData weatherData)
        {
            _weatherData = weatherData;
        }
    }
}