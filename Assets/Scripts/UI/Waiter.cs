using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 90f;
    private CancellationTokenSource _cancellationTokenSource;
    
    private void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        RotateAsync(_cancellationTokenSource.Token).Forget();
    }
    
    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
    
    private async UniTaskVoid RotateAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}
