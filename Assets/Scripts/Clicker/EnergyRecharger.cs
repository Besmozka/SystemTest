using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using R3;

public class EnergyRecharger : AutoExecuterAsync
{
    protected override int Delay => (int)_clickerData.EnergyIncreaseTime;
    
    private ClickerData _clickerData;
    
    public EnergyRecharger(ClickerData clickerData)
    {
        _clickerData = clickerData;
    }
}
