using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetMostFrequentWindDirections
{
    public class GetMostFrequentWindDirectionsHandler : CalculationHandlerBase<Dictionary<string, int>>
    {
        public GetMostFrequentWindDirectionsHandler(ITyphoonDataProvider typhoonDataProvider) : base(typhoonDataProvider)
        {
        }

        protected override Dictionary<string, int> CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            return data.Where(x => !string.IsNullOrEmpty(x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                            !x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
                .GroupBy(x => x.DirectionOfTheLongestRadiusOf50ktWindsOrGreater!)
                .ToDictionary(x => x.Key, x => x.GroupBy(i => i.InternationalNumberID).Count());
        }

        protected override Dictionary<string, int> CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var groups = new Dictionary<string, int>();

            Parallel.ForEach(data, i =>
            {
                if (!string.IsNullOrEmpty(i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater) &&
                    !i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("No direction") &&
                    !i.DirectionOfTheLongestRadiusOf50ktWindsOrGreater.Contains("(symmetric circle)"))
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
            });

            return groups;
        }

        protected override Dictionary<string, int> CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
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
