using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WeatherStats.WebApi.Handlers;
using WeatherStats.WebApi.Handlers.GetAverageNumberOfTyphoons;
using WeatherStats.WebApi.Handlers.GetAverageWindStrengthOfTyphoons;
using WeatherStats.WebApi.Handlers.GetMostCalmDates;
using WeatherStats.WebApi.Handlers.GetMostFrequentWindDirections;
using WeatherStats.WebApi.Handlers.GetMostFrequestWindDirectionOfStrongestTyphoons;
using WeatherStats.WebApi.Handlers.GetStrongestTyphoon;

namespace WeatherStats.WebApi.Controllers
{
    [ApiController]
    [Route("typhoons")]
    [Authorize]
    public class TyphoonController : ControllerBase
    {
        private readonly GetAverageNumberOfTyphoonsHandler _getAverageNumberOfTyphoonsHandler;
        private readonly GetAverageWindStrengthOfTyphoonsHandler _getAverageWindStrengthOfTyphoonsHandler;
        private readonly GetMostCalmDatesHandler _getMostCalmDatesHandler;
        private readonly GetMostFrequentWindDirectionsHandler _getMostFrequentWindDirectionsHandler;
        private readonly GetMostFrequestWindDirectionOfStrongestTyphoonsHandler _getMostFrequestWindDirectionOfStrongestTyphoonsHandler;
        private readonly GetStrongestTyphoonHandler _getStrongestTyphoonHandler;

        public TyphoonController(
            GetAverageNumberOfTyphoonsHandler getAverageNumberOfTyphoonsHandler,
            GetAverageWindStrengthOfTyphoonsHandler getAverageWindStrengthOfTyphoonsHandler,
            GetMostCalmDatesHandler getMostCalmDatesHandler,
            GetMostFrequentWindDirectionsHandler getMostFrequentWindDirectionsHandler,
            GetMostFrequestWindDirectionOfStrongestTyphoonsHandler getMostFrequestWindDirectionOfStrongestTyphoonsHandler,
            GetStrongestTyphoonHandler getStrongestTyphoonHandler
        )
        {
            _getAverageNumberOfTyphoonsHandler = getAverageNumberOfTyphoonsHandler;
            _getAverageWindStrengthOfTyphoonsHandler = getAverageWindStrengthOfTyphoonsHandler;
            _getMostCalmDatesHandler = getMostCalmDatesHandler;
            _getMostFrequentWindDirectionsHandler = getMostFrequentWindDirectionsHandler;
            _getMostFrequestWindDirectionOfStrongestTyphoonsHandler = getMostFrequestWindDirectionOfStrongestTyphoonsHandler;
            _getStrongestTyphoonHandler = getStrongestTyphoonHandler;
        }

        [HttpGet("averagecount")]
        public CalculationResponse<Dictionary<int, int>?> GetAverageNumberOfTyphoonsAsync([FromQuery] ECalculationMode mode)
        {
            return _getAverageNumberOfTyphoonsHandler.Handle(mode);
        }

        [HttpGet("averagewindstrength")]
        public CalculationResponse<Dictionary<int, double>?> GetAverageWindStrengthOfTyphoonsAsunc([FromQuery] ECalculationMode mode)
        {
            return _getAverageWindStrengthOfTyphoonsHandler.Handle(mode);
        }

        [HttpGet("mostcalmdates")]
        public CalculationResponse<List<DateTime>?> GetMostCalmDatesAsync([FromQuery] ECalculationMode mode)
        {
            return _getMostCalmDatesHandler.Handle(mode);
        }

        [HttpGet("winddirections")]
        public CalculationResponse<Dictionary<string, int>?> GetMostFrequentWindDirectionsAsync([FromQuery] ECalculationMode mode)
        {
            return _getMostFrequentWindDirectionsHandler.Handle(mode);
        }

        [HttpGet("winddirections/strongest")]
        public CalculationResponse<Dictionary<string, int>?> GetMostFrequestWindDirectionOfStrongestTyphoonsAsync([FromQuery] ECalculationMode mode)
        {
            return _getMostFrequestWindDirectionOfStrongestTyphoonsHandler.Handle(mode);
        }

        [HttpGet("strongest")]
        public CalculationResponse<List<Typhoon>?> GetStrongestTyphoonAsync([FromQuery] ECalculationMode mode)
        {
            return _getStrongestTyphoonHandler.Handle(mode);
        }
    }
}
