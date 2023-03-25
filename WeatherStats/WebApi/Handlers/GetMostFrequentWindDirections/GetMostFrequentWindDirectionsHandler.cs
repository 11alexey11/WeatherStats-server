using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetMostFrequentWindDirections
{
    public class GetMostFrequentWindDirectionsHandler
    {
        private readonly ITyphoonDataProvider _typhoonDataProvider;

        public GetMostFrequentWindDirectionsHandler(ITyphoonDataProvider typhoonDataProvider)
        {
            _typhoonDataProvider = typhoonDataProvider;
        }

        public Task<GetMostFrequentWindDirectionsResponse> HandleAsync(ECalculationMode calculationMode)
        {
            var data = _typhoonDataProvider.GetTyphoonData();

            var stopwatch = Stopwatch.StartNew();

            var result = calculationMode == ECalculationMode.Linq
                ? CalcWithLinq(data)
                : CalcWithPLinq(data);

            stopwatch.Stop();

            return Task.FromResult(
                new GetMostFrequentWindDirectionsResponse
                {
                    CalculationTime = stopwatch.ElapsedMilliseconds,
                    Directions = result,
                });
        }

        private Dictionary<string, int> CalcWithLinq(List<TyphoonDataItem> data)
        {
            return data.Where(x => !string.IsNullOrEmpty(x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
                .GroupBy(x => x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater!)
                .ToDictionary(x => x.Key, x => x.GroupBy(i => i.InternationalNumberID).Count());
        }

        private Dictionary<string, int> CalcWithPLinq(List<TyphoonDataItem> data)
        {
            return data.AsParallel()
                .Where(x => !string.IsNullOrEmpty(x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
                .GroupBy(x => x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater!)
                .ToDictionary(x => x.Key, x => x.AsParallel().GroupBy(i => i.InternationalNumberID).Count());
        }
    }
}
