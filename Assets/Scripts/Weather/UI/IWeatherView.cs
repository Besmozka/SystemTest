namespace Weather
{
    public interface IWeatherView
    {
        public void ShowTemperatureText(string text);
        public void HideTemperatureText();
        public void SetActive(bool active);
    }
}