using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weather;

public class WeatherView : MonoBehaviour, IWeatherView
{
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private GameObject temperatureBlock;

    public void ShowTemperatureText(string text)
    {
        temperatureBlock.SetActive(true);
        temperatureText.text = text;
    }

    public void HideTemperatureText()
    {
        temperatureBlock.SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
