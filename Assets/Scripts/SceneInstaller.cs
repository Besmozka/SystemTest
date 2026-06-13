using System.Threading;
using Clicker;
using DefaultNamespace;
using Dogs;
using Server;
using UnityEngine;
using Weather;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [Header("Tab views")]
    [SerializeField] private GameObject _clickerViewPrefab;
    [SerializeField] private GameObject _weatherViewPrefab;
    [SerializeField] private GameObject _dogsViewPrefab;
    
    [Header("Common")]
    [SerializeField] private NavigationPanel _navigationPanel;
    [SerializeField] private BlockPanel _blockPanel;
    [SerializeField] private GameObject _popUpPanelPrefab;
    [SerializeField] private GameObject _effectsItemPrefab;
    
    public override void InstallBindings()
    {
        InstallFisrtDependency();
        
        InstallClickerDependency();

        InstallWeatherDependency();

        InstallDogsTabDependency();

        InstallLastDependency();
    }

    private void InstallFisrtDependency()
    {
        Container.Bind<INavigationPanel>().FromInstance(_navigationPanel).AsSingle();
        Container.Bind<IBlockPanel>().FromInstance(_blockPanel).AsSingle();
        
        Container.BindInterfacesAndSelfTo<RequestsController>().FromNew().AsSingle();
    }

    private void InstallClickerDependency()
    {
        Container.Bind<IClickerView>().FromComponentInNewPrefab(_clickerViewPrefab).UnderTransformGroup("Canvas/Panels").AsSingle();
        
        Container.BindMemoryPool<EffectsItem, EffectItemsPool>().WithMaxSize(50)
            .FromComponentInNewPrefab(_effectsItemPrefab).UnderTransformGroup("Canvas/Effects");

        Container.Bind<IEnergyModel>().To<EnergyModel>().FromNew().AsSingle();
        Container.Bind<IGoldModel>().To<GoldModel>().FromNew().AsSingle();
        
        Container.Bind<IAutoExecuter>().WithId("AutoClickerService").To<AutoClickerService>().FromNew().AsSingle();
        Container.Bind<IAutoExecuter>().WithId("EnergyRecharger").To<EnergyRecharger>().FromNew().AsSingle();
        
        Container.BindInterfacesAndSelfTo<VFXController>().AsSingle();
        Container.BindInterfacesAndSelfTo<ClickerTabController>().AsSingle();
    }
    
    private void InstallWeatherDependency()
    {
        Container.Bind<IWeatherView>().FromComponentInNewPrefab(_weatherViewPrefab).UnderTransformGroup("Canvas/Panels").AsSingle();

        Container.Bind<IWeatherModel>().To<WeatherModel>().FromNew().AsSingle();
        
        Container.Bind<IAutoExecuter>().WithId("AutoWeatherRequest").To<AutoWeatherRequest>().FromNew().AsSingle();
        
        Container.BindInterfacesAndSelfTo<WeatherTabController>().AsSingle();
    }

    private void InstallDogsTabDependency()
    {
        Container.Bind<IDogsView>().FromComponentInNewPrefab(_dogsViewPrefab).UnderTransformGroup("Canvas/Panels").AsSingle();
        Container.Bind<IPopUpPanel>().FromComponentInNewPrefab(_popUpPanelPrefab).UnderTransformGroup("Canvas/Panels/DogsPanel(Clone)").AsSingle();

        Container.BindInterfacesAndSelfTo<DogsTabController>().AsSingle();
    }

    private void InstallLastDependency()
    {
        Container.BindInterfacesAndSelfTo<TabsController>().FromNew().AsSingle();
    }
}