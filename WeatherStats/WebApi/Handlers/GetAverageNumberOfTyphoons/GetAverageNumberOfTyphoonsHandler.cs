using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetAverageNumberOfTyphoons
{
    public class GetAverageNumberOfTyphoonsHandler : CalculationHandlerBase<Dictionary<int, int>>
    {
        public GetAverageNumberOfTyphoonsHandler(ITyphoonDataProvider typhoonDataProvider) : base(typhoonDataProvider)
        {
        }

        protected override Dictionary<int, int> CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            return data.GroupBy(i => i.Year)
                       .ToDictionary(
                            x => x.Key,
                            x => x.GroupBy(i => i.InternationalNumberID).Count()
                        );
        }

        protected override Dictionary<int, int> CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            return data.AsParallel()
                       .GroupBy(i => i.Year)
                       .ToDictionary(
                            x => x.Key,
                            x => x.AsParallel().GroupBy(i => i.InternationalNumberID).Count()
                        ); ;
        }

        protected override Dictionary<int, int> CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var sync = new object();
            var yearGroups = new Dictionary<int, HashSet<int>>();

            Parallel.ForEach(data, i =>
            {
                lock (sync)
                {
                    if (yearGroups.TryGetValue(i.Year, out var typhoonIds))
                    {
                        typhoonIds.Add(i.InternationalNumberID);
                    }
                    else
                    {
                        typhoonIds = new HashSet<int> { i.InternationalNumberID };
                        yearGroups[i.Year] = typhoonIds;
                    }
                }

            });

            var typhoonCounts = new Dictionary<int, int>();

            Parallel.ForEach(yearGroups, i =>
            {
                typhoonCounts[i.Key] = i.Value.Count;
            });

            return typhoonCounts;
        }
    }
}
