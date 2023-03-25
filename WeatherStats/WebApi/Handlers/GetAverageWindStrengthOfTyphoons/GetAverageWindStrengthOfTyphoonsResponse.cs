namespace WeatherStats.WebApi.Handlers.GetAverageWindStrengthOfTyphoons
{
    public class GetAverageWindStrengthOfTyphoonsResponse
    {
        public Dictionary<int, double> WindStrengthes { get; set; }
        public long CalculationTime { get; set; }
    }
}
