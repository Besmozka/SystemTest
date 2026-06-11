using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

public class EnergyRecharger : IEnergyRecharger
{
    private Subject<Unit> _onEnergyRecharge = new ();
    public Subject<Unit> OnEnergyRecharge => _onEnergyRecharge;
    
    private ClickerData _clickerData;

    public EnergyRecharger(ClickerData clickerData)
    {
        _clickerData = clickerData;
    }

    public async UniTaskVoid ChargeEnergy(CancellationToken ct)
    {
        while (true)
        {
            if (ct.IsCancellationRequested) break;

            await UniTask.Delay((int)_clickerData.EnergyIncreaseTime * 1000, cancellationToken: ct);
        
            _onEnergyRecharge.OnNext(Unit.Default);
        }
    }
}
