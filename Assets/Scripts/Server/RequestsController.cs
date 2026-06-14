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
        private List<BackendRequest> _requestQueue;
        private BackendRequest _currentRequest;

        private bool _isProcessing;

        private CancellationTokenSource _cts;

        public RequestsController()
        {
            _requestQueue = new List<BackendRequest>();
        }

        public void EnqueueRequest(BackendRequest request)
        {
            Debug.Log($"Enqueue request {request.GetType().Name}");
            
            _requestQueue.Insert(0, request);
            
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
                _cts?.Cancel();
                return;
            }
            
            if (_requestQueue.Count == 0 || !_requestQueue.Contains(request)) return;

            var index = _requestQueue.IndexOf(request);
            var doneRequest = _requestQueue[index];
            doneRequest.Done();
            
            _requestQueue.RemoveAt(index);
        }

        private async UniTaskVoid ProcessQueue()
        {
            _isProcessing = true;

            while (_requestQueue.Count > 0)
            {
                var lastIndex = _requestQueue.Count - 1;
                _currentRequest = _requestQueue[lastIndex];
                _requestQueue.RemoveAt(lastIndex);

                _cts?.Dispose();
                _cts = new CancellationTokenSource();
        
                await _currentRequest.SendRequest(_cts.Token);
            }

            _isProcessing = false;
        }


        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}