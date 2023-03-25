using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetAverageWindStrengthOfTyphoons
{
    public class GetAverageWindStrengthOfTyphoonsHandler
    {
        private readonly ITyphoonDataProvider _typhoonDataProvider;

        public GetAverageWindStrengthOfTyphoonsHandler(ITyphoonDataProvider typhoonDataProvider)
        {
            _typhoonDataProvider = typhoonDataProvider;
        }

        public Task<GetAverageWindStrengthOfTyphoonsResponse> HandleAsync(ECalculationMode calculationMode)
        {
            var data = _typhoonDataProvider.GetTyphoonData();

            var stopwatch = Stopwatch.StartNew();

            var result = calculationMode == ECalculationMode.Linq
                ? CalcWithLinq(data)
                : CalcWithPLinq(data);

            stopwatch.Stop();

            return Task.FromResult(
                new GetAverageWindStrengthOfTyphoonsResponse
                {
                    WindStrengthes = result,
                    CalculationTime = stopwatch.ElapsedMilliseconds
                });
        }

        private Dictionary<int, double> CalcWithLinq(List<TyphoonDataItem> data)
        {
            return data.Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => x.Year)
                       .OrderBy(x => x.Key)
                       .ToDictionary(
                            x => x.Key, 
                            x => x.Average(i => i.MaximumSustainedWindSpeed.Value));
        }

        private Dictionary<int, double> CalcWithPLinq(List<TyphoonDataItem> data)
        {
            return data.AsParallel()
                       .Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => x.Year)
                       .OrderBy(x => x.Key)
                       .ToDictionary(
                            x => x.Key,
                            x => x.Average(i => i.MaximumSustainedWindSpeed.Value));
        }
    }
}
