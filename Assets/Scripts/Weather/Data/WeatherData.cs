using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeatherData", menuName = "WeatherDataSO")]
public class WeatherData : ScriptableObject
{
    [Header("Weather data")]
    public string WeatherApiUrl;
    public int AutoRequestTime;
}