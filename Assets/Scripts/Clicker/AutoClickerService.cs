using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using R3;

public class AutoClickerService : AutoExecuterAsync
{
    protected override int Delay => (int) _clickerData.AutoClickTime;

    private readonly ClickerData _clickerData;

    public AutoClickerService(ClickerData clickerData)
    {
        _clickerData = clickerData;
    }
}
