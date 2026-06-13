using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClickerView : MonoBehaviour, IClickerView
{
    [SerializeField] private Button clickButton;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Slider energySlider;

    private RectTransform _rectTransform;
    public RectTransform ButtonTransform => _rectTransform;
    
    private Subject<Unit> _clickedCommand = new ();
    public Subject<Unit> ClickedCommand => _clickedCommand;

    private void Start()
    {
        clickButton.OnClickAsObservable()
            .Subscribe(_clickedCommand.OnNext)
            .AddTo(this);

        _rectTransform = clickButton.GetComponent<RectTransform>();
    }

    public void UpdateGoldText(string gold)
    {
        goldText.text = gold;    
    }

    public void UpdateEnergyText(string energy)
    {
        energyText.text = energy;
    }

    public void UpdateEnergySlider(float energy)
    {
        energySlider.value = energy;
    }
}
