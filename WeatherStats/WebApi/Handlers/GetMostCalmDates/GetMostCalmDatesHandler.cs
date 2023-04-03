using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetMostCalmDates
{
    public class GetMostCalmDatesHandler : CalculationHandlerBase<List<DateTime>>
    {
        public GetMostCalmDatesHandler(ITyphoonDataProvider typhoonDataProvider) : base(typhoonDataProvider)
        {
        }

        protected override List<DateTime> CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            return data.Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => new DateTime(x.Year, x.Month, x.Day))
                       .ToDictionary(
                           x => x.Key,
                           x => x.Max(i => i.MaximumSustainedWindSpeed.Value))
                       .Where(x => x.Value == 0)
                       .Select(x => x.Key)
                       .ToList();
        }

        protected override List<DateTime> CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var calmDates = new HashSet<DateTime>();
            var excepted = new HashSet<DateTime>();

            Parallel.ForEach(data, i =>
            {
                lock (calmDates)
                {
                    if (i.MaximumSustainedWindSpeed.HasValue)
                    {
                        var date = new DateTime(i.Year, i.Month, i.Day);

                        if (i.MaximumSustainedWindSpeed.Value == 0)
                        {
                            if (!excepted.Contains(date))
                                calmDates.Add(date);
                        }
                        else
                        {
                            if (calmDates.Contains(date))
                                calmDates.Remove(date);
                            excepted.Add(date);
                        }
                    }
                }
            });

            return calmDates.ToList();
        }

        protected override List<DateTime> CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {

            return data.AsParallel()
                       .Where(x => x.MaximumSustainedWindSpeed.HasValue)
                       .GroupBy(x => new DateTime(x.Year, x.Month, x.Day))
                       .ToDictionary(
                           x => x.Key,
                           x => x.Max(i => i.MaximumSustainedWindSpeed.Value))
                       .AsParallel()
                       .Where(x => x.Value == 0)
                       .Select(x => x.Key)
                       .ToList();
        }
    }
}
