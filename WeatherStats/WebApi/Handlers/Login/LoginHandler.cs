using Microsoft.AspNetCore.Identity;

namespace WeatherStats.WebApi.Handlers.Login
{
    public class LoginHandler
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginHandler(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<LoginResponse> HandleAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Error = "User not found",
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

            return (result.Succeeded)
                ? new LoginResponse
                {
                    IsSuccess = true,
                }
                : new LoginResponse
                {
                    IsSuccess = false,
                    Error = "Wrong password"
                };
        }
    }
}
