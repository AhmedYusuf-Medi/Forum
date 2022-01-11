using Newtonsoft.Json;

namespace Forum.Models.Response
{
    public class InfoResponse
    {
        [JsonProperty(nameof(Message))]
        public string Message { get; set; }

        [JsonProperty(nameof(IsSuccess))]
        public bool IsSuccess { get; set; }
    }
}