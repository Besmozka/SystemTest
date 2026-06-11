using System;
using System.Threading;
using Clicker.UI;
using Cysharp.Threading.Tasks;
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
        private IEnergyRecharger _energyRecharger;
        
        private IAutoClickerService _autoClickerService;
        
        private ClickerData _clickerData;

        private CancellationTokenSource _cts;
        private CompositeDisposable _disposables;

        public ClickerController(IClickerView clickerView, IGoldModel goldModel,
            IEnergyModel energyModel, IAutoClickerService autoClickerService,
            IEnergyRecharger energyRecharger, ClickerData clickerData)
        {
            _clickerView = clickerView;
            
            _goldModel = goldModel;
            _energyModel = energyModel;
            
            _autoClickerService = autoClickerService;
            _energyRecharger = energyRecharger;
            
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

            _autoClickerService.OnAutoClick
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
            
            _energyRecharger.OnEnergyRecharge
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
        }

        private void StartAutoServices()
        {
            _autoClickerService.AutoClick(_cts.Token).Forget();
            _energyRecharger.ChargeEnergy(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _disposables?.Dispose();
        }
    }
}