using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetAverageWindStrengthOfTyphoons
{
    public class GetAverageWindStrengthOfTyphoonsHandler : CalculationHandlerBase<Dictionary<int, double>>
    {
        public GetAverageWindStrengthOfTyphoonsHandler(ITyphoonDataProvider typhoonDataProvider) : base(typhoonDataProvider)
        {
        }

        protected override Dictionary<int, double> CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            return data.Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => x.Year)
                       .OrderBy(x => x.Key)
                       .ToDictionary(
                            x => x.Key,
                            x => x.Average(i => i.MaximumSustainedWindSpeed.Value));
        }

        protected override Dictionary<int, double> CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var sync = new object();
            var yearGroups = new Dictionary<int, List<double>>();

            Parallel.ForEach(data, i =>
            {
                lock (sync)
                {
                    if (i.MaximumSustainedWindSpeed.HasValue)
                    {
                        if (yearGroups.TryGetValue(i.Year, out var speeds))
                        {
                            speeds.Add(i.MaximumSustainedWindSpeed.Value);
                        }
                        else
                        {
                            speeds = new List<double>() { i.MaximumSustainedWindSpeed.Value };
                            yearGroups[i.Year] = speeds;
                        }
                    }
                }
            });

            var averageSpeeds = new Dictionary<int, double>();

            Parallel.ForEach(yearGroups, i =>
            {
                var sync = new object();

                var sum = 0d;

                Parallel.ForEach(i.Value, i =>
                {
                    lock (sync)
                    {
                        sum = sum + i;
                    }
                });

                averageSpeeds[i.Key] = sum / i.Value.Count;
            });

            return averageSpeeds;
        }

        protected override Dictionary<int, double> CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
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
