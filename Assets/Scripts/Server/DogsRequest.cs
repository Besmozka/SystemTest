using UnityEngine;

namespace Server
{
    public class DogsRequest : BackendRequest
    {
        protected override string RequestUrl => _url ?? string.Empty;
        
        private readonly string _url;

        public DogsRequest(string url)
        {
            _url = url;
        }
        
        protected override void ProcessData(string data)
        {
            Debug.Log($"Dogs text {data}");
        }
    }
}