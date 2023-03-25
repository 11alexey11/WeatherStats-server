namespace WeatherStats.WebApi.Handlers.GetMostCalmDates
{
    public class GetMostCalmDatesResponse
    {
        public long CalculationTime { get; set; }
        public Dictionary<DateTime, double> CalmDates { get; set;}
    }
}
