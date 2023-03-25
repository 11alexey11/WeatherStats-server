using System.Diagnostics;
using WeatherStats.Data;
using WeatherStats.Data.Models;

namespace WeatherStats.WebApi.Handlers.GetAverageNumberOfTyphoons
{
    public class GetAverageNumberOfTyphoonsHandler
    {
        private readonly ITyphoonDataProvider _dataProvider;

        public GetAverageNumberOfTyphoonsHandler(ITyphoonDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public Task<GetAverageNumberOfTyphoonsResponse> HandleAsync(ECalculationMode calculationMode)
        {
            var data = _dataProvider.GetTyphoonData();

            var stopwatch = Stopwatch.StartNew(); 

            var typhoons = calculationMode == ECalculationMode.Linq 
                ? CalcWithLinq(data)
                : CalcWithPLinq(data);

            stopwatch.Stop();

            return Task.FromResult(
                new GetAverageNumberOfTyphoonsResponse
                {
                    AverageTyphoonCountPerYear = typhoons,
                    CalculationTime = stopwatch.ElapsedMilliseconds
                });
        }

        private Dictionary<int, int> CalcWithLinq(List<TyphoonDataItem> data)
        {
            return data.GroupBy(i => i.Year)
                       .ToDictionary(
                            x => x.Key, 
                            x => x.GroupBy(i => i.InternationalNumberID).Count()
                        );
        }

        private Dictionary<int, int> CalcWithPLinq(List<TyphoonDataItem> data)
        {
            return data.AsParallel()
                       .GroupBy(i => i.Year)
                       .ToDictionary(
                            x => x.Key,
                            x =>  x.AsParallel().GroupBy(i => i.InternationalNumberID).Count()
                        ); ;
        }
    }
}
