using System.Diagnostics;
using WeatherStats.Data.Models;
using WeatherStats.Data;

namespace WeatherStats.WebApi.Handlers
{
    public abstract class CalculationHandlerBase<TCalculationResult> where TCalculationResult : class
    {
        private readonly ITyphoonDataProvider _typhoonDataProvider;
        public CalculationHandlerBase(ITyphoonDataProvider typhoonDataProvider)
        {
            _typhoonDataProvider = typhoonDataProvider;
        }

        public CalculationResponse<TCalculationResult?> Handle(ECalculationMode calculationMode)
        {
            var typhoonData = _typhoonDataProvider.GetTyphoonData();
            var typhoonInfo = _typhoonDataProvider.GetTyphoonInfo();

            var stopwatch = Stopwatch.StartNew();

            var calculationResult = calculationMode switch
            {
                ECalculationMode.Linq => CalculateWithLinq(typhoonData, typhoonInfo),
                ECalculationMode.PLinq => CalculateWithPLinq(typhoonData, typhoonInfo),
                ECalculationMode.Parallel => CalculateWithParallel(typhoonData, typhoonInfo),
                _ => null
            };

            stopwatch.Stop();

            return new CalculationResponse<TCalculationResult?>
            {
                CalculationTime = stopwatch.ElapsedMilliseconds,
                Payload = calculationResult,
            };
        }

        protected abstract TCalculationResult CalculateWithLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info);
        protected abstract TCalculationResult CalculateWithPLinq(List<TyphoonDataItem> data, List<TyphoonInfoItem> info);
        protected abstract TCalculationResult CalculateWithParallel(List<TyphoonDataItem> data, List<TyphoonInfoItem> info);
    }

    public class CalculationResponse<TPayload>
    {
        public long CalculationTime { get; set; }
        public TPayload Payload { get; set; }
    }
}
