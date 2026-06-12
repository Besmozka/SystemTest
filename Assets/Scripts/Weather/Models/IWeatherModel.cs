using R3;

namespace Weather
{
    public interface IWeatherModel
    {
        public ReadOnlyReactiveProperty<int> Temperature { get; }
        public void SetTemperature(int weather);
    }
}