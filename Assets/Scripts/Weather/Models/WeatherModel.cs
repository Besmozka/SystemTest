using R3;

namespace Weather
{
    public class WeatherModel : IWeatherModel
    {
        private ReactiveProperty<int> _temperature;
        public ReadOnlyReactiveProperty<int> Temperature => _temperature;

        public WeatherModel()
        {
            _temperature = new ReactiveProperty<int>();
        }

        public void SetTemperature(int weather)
        {
            _temperature.OnNext(weather);
        }
    }
}