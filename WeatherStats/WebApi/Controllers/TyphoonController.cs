using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public Task<GetAverageNumberOfTyphoonsResponse> GetAverageNumberOfTyphoonsAsync([FromQuery]ECalculationMode mode)
        {
            return _getAverageNumberOfTyphoonsHandler.HandleAsync(mode);
        }

        [HttpGet("averagewindstrength")]
        public Task<GetAverageWindStrengthOfTyphoonsResponse> GetAverageWindStrengthOfTyphoonsAsunc([FromQuery]ECalculationMode mode)
        {
            return _getAverageWindStrengthOfTyphoonsHandler.HandleAsync(mode);
        }

        [HttpGet("mostcalmdates")]
        public Task<GetMostCalmDatesResponse> GetMostCalmDatesAsync([FromQuery]ECalculationMode mode)
        {
            return _getMostCalmDatesHandler.HandleAsync(mode);
        }

        [HttpGet("winddirections")]
        public Task<GetMostFrequentWindDirectionsResponse> GetMostFrequentWindDirectionsAsync([FromQuery]ECalculationMode mode)
        {
            return _getMostFrequentWindDirectionsHandler.HandleAsync(mode);
        }

        [HttpGet("winddirections/strongest")]
        public Task<GetMostFrequestWindDirectionOfStrongestTyphoonsResponse> GetMostFrequestWindDirectionOfStrongestTyphoonsAsync([FromQuery]ECalculationMode mode)
        {
            return _getMostFrequestWindDirectionOfStrongestTyphoonsHandler.HandleAsync(mode);
        }

        [HttpGet("strongest")]
        public Task<GetStrongestTyphoonResponse> GetStrongestTyphoonAsync([FromQuery]ECalculationMode mode)
        {
            return _getStrongestTyphoonHandler.HandleAsync(mode);
        }
    }
}
