using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;

public class AutoClickerService : IAutoClickerService
{
    private Subject<Unit> _onAutoClick = new ();
    public Subject<Unit> OnAutoClick => _onAutoClick;
    
    private ClickerData _clickerData;

    public AutoClickerService(ClickerData clickerData)
    {
        _clickerData = clickerData;
    }

    public async UniTaskVoid AutoClick(CancellationToken ct)
    {
        while (true)
        {
            if (ct.IsCancellationRequested) break;

            await UniTask.Delay((int)_clickerData.AutoClickTime * 1000, cancellationToken: ct);
        
            _onAutoClick.OnNext(Unit.Default);
        }
    }
}
