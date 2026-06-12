using Newtonsoft.Json.Linq;
using R3;
using UnityEngine;

namespace Server
{
    public class WeatherRequest : BackendRequest
    {
        protected override string RequestUrl => _url ?? string.Empty;
        
        private readonly string _url;

        private Subject<int> _onTemperatureReceived;
        public Subject<int> OnTemperatureReceived => _onTemperatureReceived;

        public WeatherRequest(string url)
        {
            _url = url;
            
            _onTemperatureReceived  = new Subject<int>();
        }
        
        protected override void ProcessData(string data)
        {
            var json = JObject.Parse(data);
            if (!json.HasValues)
            {
                Debug.Log($"Cant parse {data}");
                return;
            }
            
            var temperature = (int)json["properties"]?["periods"]?[0]?["temperature"];
            
            OnTemperatureReceived.OnNext(temperature);
        }

        public override void Dispose()
        {
            OnTemperatureReceived?.Dispose();
        }
    }
}