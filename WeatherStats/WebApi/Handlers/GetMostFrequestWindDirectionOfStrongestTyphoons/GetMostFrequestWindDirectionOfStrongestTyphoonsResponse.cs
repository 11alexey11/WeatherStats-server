namespace WeatherStats.WebApi.Handlers.GetMostFrequestWindDirectionOfStrongestTyphoons
{
    public class GetMostFrequestWindDirectionOfStrongestTyphoonsResponse
    {
        public long CalculationTime { get; set; }
        public Dictionary<string, int> Directions { get; set; }
    }
}
