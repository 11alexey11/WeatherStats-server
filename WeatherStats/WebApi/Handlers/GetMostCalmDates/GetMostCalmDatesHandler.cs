using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetMostCalmDates
{
    public class GetMostCalmDatesHandler
    {
        private readonly ITyphoonDataProvider _typhoonDataProvider;
        public GetMostCalmDatesHandler(ITyphoonDataProvider typhoonDataProvider)
        {
            _typhoonDataProvider = typhoonDataProvider;
        }

        public Task<GetMostCalmDatesResponse> HandleAsync(ECalculationMode calculationMode)
        {
            var data = _typhoonDataProvider.GetTyphoonData();

            var stopwatch = Stopwatch.StartNew();

            var result = calculationMode == ECalculationMode.Linq
                ? CalcWithLinq(data)
                : CalcWithPLinq(data);

            stopwatch.Stop();

            return Task.FromResult(
                new GetMostCalmDatesResponse
                {
                    CalculationTime = stopwatch.ElapsedMilliseconds,
                    CalmDates = result
                });
        }

        private Dictionary<DateTime, double> CalcWithLinq(List<TyphoonDataItem> data)
        {
            return data.Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => new DateTime(x.Year, x.Month, x.Day))
                       .ToDictionary(
                           x => x.Key,
                           x => x.Max(i => i.MaximumSustainedWindSpeed.Value))
                       .Where(x => x.Value == 0)
                       .ToDictionary(
                            x => x.Key,
                            x => x.Value);
        }

        private Dictionary<DateTime, double> CalcWithPLinq(List<TyphoonDataItem> data)
        {
            return data.AsParallel()
                       .Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => new DateTime(x.Year, x.Month, x.Day))
                       .ToDictionary(
                           x => x.Key,
                           x => x.Max(i => i.MaximumSustainedWindSpeed.Value))
                       .AsParallel()
                       .Where(x => x.Value == 0)
                       .ToDictionary(
                           x => x.Key,
                           x => x.Value);
        }
    }
}
