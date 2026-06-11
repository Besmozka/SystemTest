using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public interface IAutoClickerService
{
    public Subject<Unit> OnAutoClick { get; }

    public UniTaskVoid AutoClick(CancellationToken ct);
}
