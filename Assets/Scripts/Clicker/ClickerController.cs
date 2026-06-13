using System;
using System.Threading;
using Clicker;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using R3;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Clicker
{
    public class ClickerController : IDisposable
    {
        private IClickerView _clickerView;
        private IGoldModel _goldModel;
        private IEnergyModel _energyModel;
        private IAutoExecuter _energyRecharger;
        private IAutoExecuter _autoClickerService;
        private IVFXController _clickerVfx;
        private INavigationPanel _navigationPanel;
        
        private ClickerData _clickerData;

        private CancellationTokenSource _cts;
        private CompositeDisposable _disposables;

        public ClickerController(IClickerView clickerView, IGoldModel goldModel, INavigationPanel navigationPanel,
            IEnergyModel energyModel, ClickerData clickerData, IVFXController clickerVfx,
            [Inject(Id = "AutoClickerService")] IAutoExecuter autoClickerService,
            [Inject(Id = "EnergyRecharger")] IAutoExecuter energyRecharger)
        {
            _clickerView = clickerView;
            _navigationPanel = navigationPanel;
            
            _goldModel = goldModel;
            _energyModel = energyModel;
            
            _autoClickerService = autoClickerService;
            _energyRecharger = energyRecharger;
            _clickerVfx = clickerVfx;
            
            _clickerData = clickerData;

            _cts = new CancellationTokenSource();
            _disposables = new CompositeDisposable();
            
            Init();
            
            StartAutoServices();
        }

        private void Init()
        {
            _clickerView.ClickedCommand
                .AsObservable()
                .Subscribe(_ => OnClick())
                .AddTo(_disposables);

            _autoClickerService.OnExecute
                .AsObservable()
                .Subscribe(_ => OnClick())
                .AddTo(_disposables);
            
            _goldModel.CurrentGold
                .Subscribe(gold => _clickerView.UpdateGoldText(gold.ToString()))
                .AddTo(_disposables);

            _energyModel.CurrentEnergy
                .Select(energy => $"{energy} / {_energyModel.MaxEnergy}")
                .Subscribe(energy => _clickerView.UpdateEnergyText(energy))
                .AddTo(_disposables);
            
            _energyModel.CurrentEnergy
                .Select(energy => (float) energy / _energyModel.MaxEnergy)
                .Subscribe(x =>
                {
                    _clickerView.UpdateEnergySlider(x);
                })
                .AddTo(_disposables);
            
            _energyRecharger.OnExecute
                .AsObservable()
                .Subscribe(_ => AddEnergy())
                .AddTo(_disposables);
            
            _navigationPanel.WeatherCommand
                .Subscribe(_ => StartAutoServices())
                .AddTo(_disposables);
            
            _navigationPanel.ClickerCommand
                .Subscribe(_ => StopAutoServices())
                .AddTo(_disposables);
            _navigationPanel.DogsCommand
                .Subscribe(_ => StopAutoServices())
                .AddTo(_disposables);
        }

        private void AddEnergy()
        {
            _energyModel.AddEnergy(_clickerData.EnergyChargeValue);
        }

        private void OnClick()
        {
            _goldModel.AddGold(_clickerData.ClickGoldCost);
            _energyModel.RemoveEnergy(_clickerData.ClickEnergyCost);
            
            _clickerVfx.SpawnEffects(_clickerView.ButtonTransform);
        }

        private void StartAutoServices()
        {
            _autoClickerService.Execute(_cts.Token).Forget();
            _energyRecharger.Execute(_cts.Token).Forget();
        }

        private void StopAutoServices()
        {
            _cts.Cancel();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _disposables?.Dispose();
        }
    }
}