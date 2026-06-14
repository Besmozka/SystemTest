using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

public interface IAutoExecuter
{
    public Subject<Unit> OnExecute { get; }
    public UniTaskVoid Execute(CancellationToken ct);
}