using System.Threading;
using Clicker;
using DefaultNamespace;
using Server;
using UnityEngine;
using Weather;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ClickerView _clickerView;
    [SerializeField] private WeatherView _weatherView;
    
    
    [SerializeField] private NavigationPanel _navigationPanel;
    
    private CancellationTokenSource _cts;
    
    public override void InstallBindings()
    {
        _cts = new CancellationTokenSource();

        InstallCommonDependency();
        
        InstallClickerDependency();

        InstallWeatherDependency();
    }

    private void InstallCommonDependency()
    {
        Container.Bind<INavigationPanel>().To<NavigationPanel>().FromInstance(_navigationPanel).AsSingle();
        Container.BindInterfacesAndSelfTo<CancellationToken>().FromInstance(_cts.Token).AsCached();
        
        Container.BindInterfacesAndSelfTo<RequestsController>().FromNew().AsSingle();
    }

    private void InstallClickerDependency()
    {
        Container.Bind<IClickerView>().FromInstance(_clickerView).AsSingle();

        Container.Bind<IEnergyModel>().To<EnergyModel>().FromNew().AsSingle();
        Container.Bind<IGoldModel>().To<GoldModel>().FromNew().AsSingle();
        
        Container.Bind<IAutoExecuter>().WithId("AutoClickerService").To<AutoClickerService>().FromNew().AsSingle();
        Container.Bind<IAutoExecuter>().WithId("EnergyRecharger").To<EnergyRecharger>().FromNew().AsSingle();
        
        Container.BindInterfacesAndSelfTo<ClickerController>().AsSingle();
    }
    
    private void InstallWeatherDependency()
    {
        Container.Bind<IWeatherView>().FromInstance(_weatherView).AsSingle();

        Container.Bind<IWeatherModel>().To<WeatherModel>().FromNew().AsSingle();
        
        Container.Bind<IAutoExecuter>().WithId("AutoWeatherRequest").To<AutoWeatherRequest>().FromNew().AsSingle();
        
        Container.BindInterfacesAndSelfTo<WeatherController>().AsSingle();
    }
}