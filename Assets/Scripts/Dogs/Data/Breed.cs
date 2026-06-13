using Newtonsoft.Json;

public class Breed
{
    [JsonProperty("id")]
    public string Id { get; private set; }
    
    [JsonProperty("attributes")]
    public Attributes Info { get; private set; }

    public class Attributes
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}