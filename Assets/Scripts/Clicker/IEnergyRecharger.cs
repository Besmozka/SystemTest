using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public interface IEnergyRecharger
{
    public Subject<Unit> OnEnergyRecharge { get; }
    public UniTaskVoid ChargeEnergy(CancellationToken ct);
}
