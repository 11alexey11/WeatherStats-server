using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WeatherStats.WebApi.Handlers.Login;
using Microsoft.AspNetCore.Authorization;

namespace WeatherStats.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("entrance")]
    [ApiController]
    public class EntranceController : ControllerBase
    {
        private readonly LoginHandler _loginHandler;

        public EntranceController(LoginHandler loginHandler) 
        {
            _loginHandler = loginHandler;
        }

        [HttpPost("login")]
        public Task<LoginResponse> LoginAsync([FromBody]LoginRequest request)
        {
            return _loginHandler.HandleAsync(request);
        }
    }
}
