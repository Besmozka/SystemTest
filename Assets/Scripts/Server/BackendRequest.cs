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
        
        public async UniTask SendRequest(CancellationToken ct)
        {
            Debug.Log($"Send request {this.GetType().Name}, URL: {RequestUrl}");
            
            var requestUrl = RequestUrl;
            
            if (string.IsNullOrEmpty(requestUrl))
            {
                CompleteRequest(null);
                return;
            }

            _request = UnityWebRequest.Get(requestUrl);
            
            await _request.SendWebRequest().WithCancellation(ct);
            
            if (_request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка: {_request.error}");
            }
            else
            {
                CompleteRequest(_request.downloadHandler.text);
            }
        }

        private void CompleteRequest(string data)
        {
            ProcessData(data);
            Done();
        }

        public void Done()
        {
            _request?.Abort();
            _request?.Dispose();
            _request = null;
        }

        public virtual void Dispose()
        {
            _request?.Abort();
            _request?.Dispose();
        }
    }
}