using Newtonsoft.Json;

namespace WeatherStats.WebApi
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Error { get; set; }
    }

    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T? Payload { get; set; }
    }
}
