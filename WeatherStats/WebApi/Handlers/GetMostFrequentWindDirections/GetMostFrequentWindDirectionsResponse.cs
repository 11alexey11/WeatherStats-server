namespace WeatherStats.WebApi.Handlers.GetMostFrequentWindDirections
{
    public class GetMostFrequentWindDirectionsResponse
    {
        public long CalculationTime { get; set; }
        public Dictionary<string, int> Directions { get; set; }
    }
}
