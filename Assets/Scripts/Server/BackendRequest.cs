using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    public abstract class BackendRequest : IDisposable
    {
        private UnityWebRequest _request;
        protected abstract string RequestUrl { get; }
        protected abstract void ProcessData(string data);
        private bool _isProcessing;
        
        public async UniTask SendRequest(CancellationToken ct)
        {
            if (_isProcessing) return;
            _isProcessing = true;
            
            Debug.Log($"Send request {this.GetType().Name}, URL {RequestUrl}");
    
            var requestUrl = RequestUrl;
    
            if (string.IsNullOrEmpty(requestUrl))
            {
                CompleteRequest(null);
                return;
            }
            
            _request = UnityWebRequest.Get(requestUrl);
    
            try
            {
                await _request.SendWebRequest().WithCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"Request canceled {this.GetType().Name}");
                return;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }
            
            CompleteRequest(_request.downloadHandler.text);
        }

        private void CompleteRequest(string data)
        {
            ProcessData(data);
            Done();
        }
        
        public void Done()
        {
            if (_isProcessing)
            {
                _request?.Abort();
                _request?.Dispose();
            }

            _isProcessing = false;
        }

        public virtual void Dispose()
        {
            _request?.Abort();
            _request?.Dispose();
        }
    }
}