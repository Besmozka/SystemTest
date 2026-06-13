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
    
    [SerializeField] private GameObject _effectsItemPrefab;
    
    public override void InstallBindings()
    {
        InstallCommonDependency();
        
        InstallClickerDependency();

        InstallWeatherDependency();
    }

    private void InstallCommonDependency()
    {
        Container.Bind<INavigationPanel>().FromInstance(_navigationPanel).AsSingle();
        
        Container.BindInterfacesAndSelfTo<RequestsController>().FromNew().AsSingle();
    }

    private void InstallClickerDependency()
    {
        Container.BindMemoryPool<EffectsItem, EffectItemsPool>().WithMaxSize(50)
            .FromComponentInNewPrefab(_effectsItemPrefab).UnderTransformGroup("Canvas").AsSingle();
        
        Container.Bind<IClickerView>().FromInstance(_clickerView).AsSingle();

        Container.Bind<IEnergyModel>().To<EnergyModel>().FromNew().AsSingle();
        Container.Bind<IGoldModel>().To<GoldModel>().FromNew().AsSingle();
        
        Container.Bind<IAutoExecuter>().WithId("AutoClickerService").To<AutoClickerService>().FromNew().AsSingle();
        Container.Bind<IAutoExecuter>().WithId("EnergyRecharger").To<EnergyRecharger>().FromNew().AsSingle();
        
        Container.BindInterfacesAndSelfTo<VFXController>().AsSingle();
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