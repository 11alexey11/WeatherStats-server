using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetStrongestTyphoon
{
    public class GetStrongestTyphoonHandler
    {
        private ITyphoonDataProvider _typhoonDataProvider;

        public GetStrongestTyphoonHandler(ITyphoonDataProvider typhoonDataProvider)
        {
            _typhoonDataProvider = typhoonDataProvider;
        }

        public Task<GetStrongestTyphoonResponse> HandleAsync(ECalculationMode calculationMode)
        {
            var data = _typhoonDataProvider.GetTyphoonData();
            var info = _typhoonDataProvider.GetTyphoonInfo();

            var stopwatch = Stopwatch.StartNew();

            var result = calculationMode == ECalculationMode.Linq
                ? CalcWithLinq(data, info)
                : CalcWithPLinq(data, info);

            stopwatch.Stop();

            return Task.FromResult(new GetStrongestTyphoonResponse()
            {
                CalculationTime = stopwatch.ElapsedMilliseconds,
                Typhoons = result
            });
        }

        private List<Typhoon> CalcWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
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

        private List<Typhoon> CalcWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info)
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
