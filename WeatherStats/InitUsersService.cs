using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherStats
{
    public class InitUsersService : IHostedService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public InitUsersService(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.CreateScope()
                                          .ServiceProvider
                                          .GetRequiredService<UserManager<IdentityUser>>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _userManager.CreateAsync(
                new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                },
                "12345");

            await _userManager.CreateAsync(
                new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Guest",
                    NormalizedUserName = "GUEST",
                },
                "123");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
