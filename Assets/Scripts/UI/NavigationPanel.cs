using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : MonoBehaviour, INavigationPanel
{
    [SerializeField] private Button clickerButton;
    [SerializeField] private Button weatherButton;
    [SerializeField] private Button dogsButton;
    
    private Subject<Unit> _clickerButton = new ();
    public Subject<Unit> ClickerCommand => _clickerButton;
    
    private Subject<Unit> _weatherButton = new ();
    public Subject<Unit> WeatherCommand => _weatherButton;
    
    private Subject<Unit> _dogsButton = new ();
    public Subject<Unit> DogsCommand => _dogsButton;

    private void Start()
    {
        clickerButton.OnClickAsObservable()
            .Subscribe(_clickerButton.OnNext)
            .AddTo(this);
        
        weatherButton.OnClickAsObservable()
            .Subscribe(_weatherButton.OnNext)
            .AddTo(this);
        
        dogsButton.OnClickAsObservable()
            .Subscribe(_dogsButton.OnNext)
            .AddTo(this);
    }
}
