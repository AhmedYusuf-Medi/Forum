using Newtonsoft.Json;

namespace Forum.Models.Response
{
    public class Response<T> : InfoResponse
    {
        [JsonProperty(nameof(Payload))]
        public T Payload { get; set; }
    }
}