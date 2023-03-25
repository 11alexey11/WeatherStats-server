namespace WeatherStats.WebApi.Handlers.GetAverageNumberOfTyphoons
{
    public class GetAverageNumberOfTyphoonsResponse
    {
        public long CalculationTime { get; set; }
        public Dictionary<int, int> AverageTyphoonCountPerYear { get; set; }
    }
}
