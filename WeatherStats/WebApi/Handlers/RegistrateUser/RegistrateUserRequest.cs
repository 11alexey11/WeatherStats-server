namespace WeatherStats.WebApi.Handlers.RegistrateUser
{
    public class RegistrateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
