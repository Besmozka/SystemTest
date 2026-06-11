using System.Threading;
using Clicker;
using Clicker.UI;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ClickerView _clickerView;
    [SerializeField] private NavigationPanel _navigationPanel;
    
    private CancellationTokenSource _cts;
    
    public override void InstallBindings()
    {
        _cts = new CancellationTokenSource();

        InstallCommonDependency();
        
        InstallClickerDependency();
    }

    private void InstallCommonDependency()
    {
        Container.Bind<INavigationPanel>().To<NavigationPanel>().FromInstance(_navigationPanel).AsSingle();
    }

    private void InstallClickerDependency()
    {
        Container.BindInterfacesAndSelfTo<CancellationToken>().FromInstance(_cts.Token).AsCached();
        
        Container.Bind<IClickerView>().FromInstance(_clickerView).AsSingle();

        Container.Bind<IEnergyModel>().To<EnergyModel>().FromNew().AsSingle();
        Container.Bind<IGoldModel>().To<GoldModel>().FromNew().AsSingle();
        
        Container.Bind<IAutoClickerService>().To<AutoClickerService>().FromNew().AsSingle();
        Container.Bind<IEnergyRecharger>().To<EnergyRecharger>().FromNew().AsSingle();
        
        Container.BindInterfacesAndSelfTo<ClickerController>().AsSingle();
    }
}