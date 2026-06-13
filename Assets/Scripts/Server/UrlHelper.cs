using System.Collections.Generic;
using UnityEngine.Networking;

namespace Server
{
    public class UrlHelper
    {
        private readonly string _baseUrl;
        private readonly List<string> _parameters = new List<string>();
    
        public UrlHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
    
        public UrlHelper AddParameter(string key, string value)
        {
            _parameters.Add($"{key}={UnityWebRequest.EscapeURL(value)}");
            return this;
        }
    
        public UrlHelper AddParameter(string key, int value)
        {
            return AddParameter(key, value.ToString());
        }
    
        public string Build()
        {
            if (_parameters.Count == 0)
                return _baseUrl;
            
            return $"{_baseUrl}?{string.Join("&", _parameters)}";
        }
    }
}