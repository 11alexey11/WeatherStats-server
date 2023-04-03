using Microsoft.AspNetCore.Identity;

namespace WeatherStats.WebApi.Handlers.RegistrateUser
{
    public class RegistrateUserHandler
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegistrateUserHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegistrateUserResponse> HandleAsync(RegistrateUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user != null)
            {
                return new RegistrateUserResponse
                {
                    Error = "User already exists",
                };
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new RegistrateUserResponse
                {
                    Error = "Passwords are not same",
                };
            }

            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = request.Username,
                NormalizedUserName = request.Username.ToUpper(),
            }, request.Password);

            if (!result.Succeeded)
            {
                return new RegistrateUserResponse
                {
                    Error = result.Errors.FirstOrDefault()?.Description ?? "InternalServerError"
                };
            }

            return new RegistrateUserResponse
            {
                IsSuccess = true,
            };
        }
    }
}
