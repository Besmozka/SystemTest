using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Server
{
    public class RequestsController : IRequestsController, IDisposable
    {
        private Queue<BackendRequest> _requestQueue = new Queue<BackendRequest>();
        private BackendRequest _currentRequest;

        private bool _isProcessing;

        private CancellationTokenSource _cts = new();

        public void EnqueueRequest(BackendRequest request)
        {
            Debug.Log($"Enqueue request {request.GetType().Name}");
            
            _requestQueue.Enqueue(request);
            
            if (!_isProcessing)
            {
                ProcessQueue().Forget();
            }
        }

        public void DequeueRequest(BackendRequest request)
        {
            Debug.Log($"Dequeue request {_currentRequest?.GetType().Name}");
            
            if (_currentRequest != null && _currentRequest.Equals(request))
            {
                _currentRequest.Done();
                return;
            }
            
            if (_requestQueue.Count == 0) return;
            
            _requestQueue.Dequeue().Done();
        }

        private async UniTaskVoid ProcessQueue()
        {
            _isProcessing = true;

            while (_requestQueue.Count > 0)
            {
                _currentRequest = _requestQueue.Dequeue();

                Debug.Log($"Send request {_currentRequest.GetType().Name}");
                
                await _currentRequest.SendRequest(_cts.Token);
            }

            _isProcessing = false;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts?.Dispose();
        }
    }
}