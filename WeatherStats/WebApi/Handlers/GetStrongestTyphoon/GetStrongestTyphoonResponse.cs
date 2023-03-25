namespace WeatherStats.WebApi.Handlers.GetStrongestTyphoon
{
    public class GetStrongestTyphoonResponse
    {
        public long CalculationTime { get; set; }
        public List<Typhoon> Typhoons { get; set; }
    }
}
