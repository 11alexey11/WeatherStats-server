using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WeatherStats.WebApi.Handlers.Login;
using Microsoft.AspNetCore.Authorization;
using WeatherStats.WebApi.Handlers.RegistrateUser;

namespace WeatherStats.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("entrance")]
    [ApiController]
    public class EntranceController : ControllerBase
    {
        private readonly LoginHandler _loginHandler;
        private readonly RegistrateUserHandler _registrateUserHandler;

        public EntranceController(LoginHandler loginHandler, RegistrateUserHandler registrateUserHandler) 
        {
            _loginHandler = loginHandler;
            _registrateUserHandler = registrateUserHandler;
        }

        [HttpPost("login")]
        public Task<LoginResponse> LoginAsync([FromBody]LoginRequest request)
        {
            return _loginHandler.HandleAsync(request);
        }

        [HttpPost]
        public Task<RegistrateUserResponse> RegistrateUserAsync([FromBody] RegistrateUserRequest request)
        {
            return _registrateUserHandler.HandleAsync(request);
        }
    }
}
