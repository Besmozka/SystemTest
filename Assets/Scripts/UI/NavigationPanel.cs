using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NavigationPanel : MonoBehaviour, INavigationPanel
{
    [SerializeField] private Button clickerButton;
    [SerializeField] private Button weatherButton;
    [SerializeField] private Button dogsButton;
    
    [SerializeField] private Image backgroundImage;
    
    private Subject<Unit> _clickerButton = new ();
    public Subject<Unit> ClickerCommand => _clickerButton;
    
    private Subject<Unit> _weatherButton = new ();
    public Subject<Unit> WeatherCommand => _weatherButton;
    
    private Subject<Unit> _dogsButton = new ();
    public Subject<Unit> DogsCommand => _dogsButton;
    
    [Inject]
    private BackgroundData _backgroundData; 

    private void Start()
    {
        SetBackground(TabType.Clicker);
        
        clickerButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SetBackground(TabType.Clicker);
                _clickerButton.OnNext(Unit.Default);
            })
            .AddTo(this);
        
        weatherButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SetBackground(TabType.Weather);
                _weatherButton.OnNext(Unit.Default);
            })
            .AddTo(this);
        
        dogsButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SetBackground(TabType.Dogs);
                _dogsButton.OnNext(Unit.Default);
            })
            .AddTo(this);
    }

    private void SetBackground(TabType type)
    {
        switch (type)
        {
            case TabType.Clicker:
                backgroundImage.sprite = _backgroundData.ClickerImage;
                break;
            case TabType.Weather:
                backgroundImage.sprite = _backgroundData.WeatherImage;
                break;
            case TabType.Dogs:
                backgroundImage.sprite = _backgroundData.DogsImage;
                break;
        }
    }
}

public enum TabType
{
    Clicker,
    Weather,
    Dogs
}
