using System;
using System.Threading;
using DefaultNamespace;
using R3;
using Zenject;

namespace Clicker
{
    public class ClickerTabController : IDisposable, ITabController
    {
        private IClickerView _clickerView;
        private IGoldModel _goldModel;
        private IEnergyModel _energyModel;
        private IAutoExecuter _energyRecharger;
        private IAutoExecuter _autoClickerService;
        private IVFXController _clickerVfx;
        
        private ClickerData _clickerData;

        private CancellationTokenSource _cts;
        private CompositeDisposable _disposables;

        public ClickerTabController(IClickerView clickerView, IGoldModel goldModel,
            IEnergyModel energyModel, ClickerData clickerData, IVFXController clickerVfx,
            [Inject(Id = "AutoClickerService")] IAutoExecuter autoClickerService,
            [Inject(Id = "EnergyRecharger")] IAutoExecuter energyRecharger)
        {
            _clickerView = clickerView;
            
            _goldModel = goldModel;
            _energyModel = energyModel;
            
            _autoClickerService = autoClickerService;
            _energyRecharger = energyRecharger;
            _clickerVfx = clickerVfx;
            
            _clickerData = clickerData;

            _disposables = new CompositeDisposable();
            
            Init();
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

        private void StartServices()
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            _autoClickerService.Execute(_cts.Token).Forget();
            _energyRecharger.Execute(_cts.Token).Forget();
        }

        private void StopServices()
        {
            _cts?.Cancel();
        }

        public void ShowTab()
        {
            _clickerView.SetActive(true);
            
            StartServices();
        }

        public void HideTab()
        {
            _clickerView.SetActive(false);
            
            StopServices();

            _clickerVfx.HideAllVFX();
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _disposables?.Dispose();
        }
    }
}