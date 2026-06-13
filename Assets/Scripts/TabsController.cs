using System;
using Clicker;
using R3;
using Weather;

public class TabsController : IDisposable
{
    private INavigationPanel _navigationPanel;
    private ClickerTabController _clickerTabController;
    private WeatherTabController _weatherTabController;
    private DogsTabController _dogsTabController;
    
    private ITabController _currentTabController;
    
    private CompositeDisposable _disposables;

    public TabsController(INavigationPanel navigationPanel, ClickerTabController clickerTabController,
        WeatherTabController weatherTabController, DogsTabController dogsTabController)
    {
        _navigationPanel = navigationPanel;
        _clickerTabController = clickerTabController;
        _weatherTabController = weatherTabController;
        _dogsTabController = dogsTabController;
        
        _disposables = new CompositeDisposable();

        Init();
    }

    private void Init()
    { 
        _navigationPanel.ClickerCommand
            .Subscribe(_ => ClickOnTab(TabType.Clicker))
            .AddTo(_disposables);
            
        _navigationPanel.WeatherCommand
            .Subscribe(_ => ClickOnTab(TabType.Weather))
            .AddTo(_disposables);
        
        _navigationPanel.DogsCommand
            .Subscribe(_ => ClickOnTab(TabType.Dogs))
            .AddTo(_disposables);
        
        ClickOnTab(TabType.Clicker);
    }

    private void ClickOnTab(TabType tabType)
    {
        switch (tabType)
        {
            case TabType.Clicker:
                ShowTab(_clickerTabController);
                break;
            case TabType.Weather:
                ShowTab(_weatherTabController);
                break;
            case TabType.Dogs:
                ShowTab(_dogsTabController);
                break;
        }
    }

    private void ShowTab(ITabController tabController)
    {
        _currentTabController?.HideTab();
                
        _currentTabController = tabController;
                
        _currentTabController.ShowTab();
    }

    public void Dispose()
    {
        _clickerTabController?.Dispose();
        _weatherTabController?.Dispose();
        _dogsTabController?.Dispose();
        _disposables?.Dispose();
    }
}
