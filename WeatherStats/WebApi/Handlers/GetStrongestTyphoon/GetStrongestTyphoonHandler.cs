using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetStrongestTyphoon
{
    public class GetStrongestTyphoonHandler : CalculationHandlerBase<List<Typhoon>>
    {
        public GetStrongestTyphoonHandler(ITyphoonDataProvider typhoonDataProvider) : base(typhoonDataProvider)
        {
        }

        protected override List<Typhoon> CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var speeds = data.Where(i => i.MaximumSustainedWindSpeed.HasValue)
                             .GroupBy(i => i.MaximumSustainedWindSpeed);

            var maxSpeed = speeds.Select(i => i.Key).Max();

            var strongestGroup = speeds.FirstOrDefault(i => i.Key == maxSpeed);

            var typhoonIds = strongestGroup.GroupBy(i => i.InternationalNumberID).Select(i => i.Key).ToList();

            return info.Where(i => typhoonIds.Contains(i.InternationalNumberID))
                       .Select(i => new Typhoon
                       {
                           Id = i.InternationalNumberID,
                           Date = i.LatestRevision,
                           Name = i.Name
                       }).ToList();
        }

        protected override List<Typhoon> CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var sync = new object();
            var maxSpeed = 0d;
            HashSet<int> strongestTyphoonIds = new HashSet<int>();

            Parallel.ForEach(data, i =>
            {
                lock (sync)
                {
                    if (i.MaximumSustainedWindSpeed.HasValue)
                    {
                        if (i.MaximumSustainedWindSpeed.Value > maxSpeed)
                        {
                            maxSpeed = i.MaximumSustainedWindSpeed.Value;
                            strongestTyphoonIds = new HashSet<int> { i.InternationalNumberID };
                        }
                        else
                        {
                            strongestTyphoonIds.Add(i.InternationalNumberID);
                        }
                    }
                }
            });


            return info.Where(i => i.InternationalNumberID == strongestTyphoonIds.First())
                       .Select(i => new Typhoon
                       {
                           Id = i.InternationalNumberID,
                           Date = i.LatestRevision,
                           Name = i.Name
                       }).ToList();
        }

        protected override List<Typhoon> CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
        {
            var speeds = data.AsParallel()
                             .Where(i => i.MaximumSustainedWindSpeed.HasValue)
                             .GroupBy(i => i.MaximumSustainedWindSpeed);

            var maxSpeed = speeds.Select(i => i.Key).Max();

            var strongestGroup = speeds.FirstOrDefault(i => i.Key == maxSpeed);

            var typhoonIds = strongestGroup.AsParallel()
                                           .GroupBy(i => i.InternationalNumberID)
                                           .Select(i => i.Key)
                                           .ToList();

            return info.AsParallel()
                       .Where(i => typhoonIds.Contains(i.InternationalNumberID))
                       .Select(i => new Typhoon
                       {
                           Id = i.InternationalNumberID,
                           Date = i.LatestRevision,
                           Name = i.Name
                       }).ToList();
        }
    }
}
