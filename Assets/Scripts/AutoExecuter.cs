using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace DefaultNamespace
{
    public abstract class AutoExecuterAsync : IAutoExecuter
    {
        private Subject<Unit> _onExecute = new ();
        public Subject<Unit> OnExecute => _onExecute;
        
        protected abstract int Delay { get; }
        
        private bool _isExecuting;
        
        public async UniTaskVoid Execute(CancellationToken ct)
        {
            if (_isExecuting) return;
            
            _isExecuting = true;
            
            while (true)
            {
                if (ct.IsCancellationRequested) break;
        
                await UniTask.Delay(Delay * 1000, cancellationToken: ct);
            
                _onExecute.OnNext(Unit.Default);
            }
        }
        
    }
}