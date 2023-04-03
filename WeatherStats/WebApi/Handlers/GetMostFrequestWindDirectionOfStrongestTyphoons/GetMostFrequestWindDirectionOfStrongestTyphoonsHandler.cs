using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetMostFrequestWindDirectionOfStrongestTyphoons
{
    public class GetMostFrequestWindDirectionOfStrongestTyphoonsHandler : CalculationHandlerBase<Dictionary<string, int>>
    {
        public GetMostFrequestWindDirectionOfStrongestTyphoonsHandler(ITyphoonDataProvider typhoonDataProvider) : base(typhoonDataProvider)
        {
        }

        protected override Dictionary<string, int> CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
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

        protected override Dictionary<string, int> CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var maxSpeed = 0d;
            var groups = new Dictionary<string, int>();

            Parallel.ForEach(data, i =>
            {
                lock (groups)
                {
                    if (i.MaximumSustainedWindSpeed.HasValue)
                        maxSpeed = maxSpeed < i.MaximumSustainedWindSpeed.Value
                            ? i.MaximumSustainedWindSpeed.Value
                            : maxSpeed;
                }
            });

            Parallel.ForEach(data, i =>
            {
                if (i.MaximumSustainedWindSpeed.HasValue &&
                    i.MaximumSustainedWindSpeed.Value > maxSpeed * 2 / 3 &&
                   !string.IsNullOrEmpty(i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) &&
                   !i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                   !i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
                {
                    lock (groups)
                    {
                        if (groups.TryGetValue(i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater, out var count))
                        {
                            groups[i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater] = count + 1;
                        }
                        else
                        {
                            groups[i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater] = 1;
                        }
                    }
                }
            });

            return groups;
        }

        protected override Dictionary<string, int> CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
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
