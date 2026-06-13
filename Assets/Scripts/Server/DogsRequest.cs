using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using R3;
using UnityEngine;

namespace Server
{
    public class DogsRequest : BackendRequest
    {
        protected override string RequestUrl => _url ?? string.Empty;
        
        private readonly string _url;
        
        private Subject<List<Breed>> _onRecieveBreeds;
        public Subject<List<Breed>> OnRecieveBreeds => _onRecieveBreeds;
        
        private Subject<Breed> _onRecieveBreedDescription;
        public Subject<Breed> OnRecieveBreedDescription => _onRecieveBreedDescription;

        public DogsRequest(string url)
        {
            _url = url;
            
            _onRecieveBreeds = new Subject<List<Breed>>();
            _onRecieveBreedDescription = new Subject<Breed>();
        }
        
        protected override void ProcessData(string data)
        {
            var jObject = JObject.Parse(data);
            var dataObject = jObject["data"];

            if (dataObject?.Type == JTokenType.Array)
            {
                var breeds = dataObject.ToObject<List<Breed>>();
                _onRecieveBreeds.OnNext(breeds);
            }
            else
            {
                var breed = dataObject?.ToObject<Breed>();
                _onRecieveBreedDescription.OnNext(breed);
            }
        }
    }
}