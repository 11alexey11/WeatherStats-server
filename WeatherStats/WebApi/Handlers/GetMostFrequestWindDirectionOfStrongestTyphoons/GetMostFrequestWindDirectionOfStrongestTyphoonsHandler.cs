using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetMostFrequestWindDirectionOfStrongestTyphoons
{
    public class GetMostFrequestWindDirectionOfStrongestTyphoonsHandler
    {
        private readonly ITyphoonDataProvider _typhoonDataProvider;

        public GetMostFrequestWindDirectionOfStrongestTyphoonsHandler(ITyphoonDataProvider typhoonDataProvider)
        {
            _typhoonDataProvider = typhoonDataProvider;
        }

        public Task<GetMostFrequestWindDirectionOfStrongestTyphoonsResponse> HandleAsync(ECalculationMode calculationMode)
        {
            var data = _typhoonDataProvider.GetTyphoonData();

            var stopwatch = Stopwatch.StartNew();
            var result = calculationMode == ECalculationMode.Linq
                ? CalcWithLinq(data)
                : CalcWithPLinq(data);
            stopwatch.Stop();

            return Task.FromResult(
                new GetMostFrequestWindDirectionOfStrongestTyphoonsResponse
                {
                    CalculationTime = stopwatch.ElapsedMilliseconds,
                    Directions = result
                });
        }

        private Dictionary<string, int> CalcWithLinq(List<TyphoonDataItem> data)
        {
            var maxSpeed = data.Max(x => x.MaximumSustainedWindSpeed);
            return data.Where(x => x.MaximumSustainedWindSpeed.HasValue && 
                            x.MaximumSustainedWindSpeed > maxSpeed / 3 * 2 && 
                            !string.IsNullOrEmpty(x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) && 
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
                .GroupBy(x => x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater!)
                .ToDictionary(x => x.Key, x => x.GroupBy(i => i.InternationalNumberID).Count());
        }

        private Dictionary<string, int> CalcWithPLinq(List<TyphoonDataItem> data)
        {
            var maxSpeed = data.AsParallel().Max(x => x.MaximumSustainedWindSpeed);
            return data.AsParallel().Where(x => x.MaximumSustainedWindSpeed.HasValue &&
                            x.MaximumSustainedWindSpeed > maxSpeed / 3 * 2 &&
                            !string.IsNullOrEmpty(x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
                .GroupBy(x => x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater!)
                .ToDictionary(x => x.Key, x => x.AsParallel().GroupBy(i => i.InternationalNumberID).Count());
        }
    }
}
