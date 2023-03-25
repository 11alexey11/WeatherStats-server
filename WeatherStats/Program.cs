using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WeatherStats;
using WeatherStats.Data;
using WeatherStats.Data.Models;
using WeatherStats.Identity.Strores;
using WeatherStats.WebApi.Handlers.GetAverageNumberOfTyphoons;
using WeatherStats.WebApi.Handlers.GetAverageWindStrengthOfTyphoons;
using WeatherStats.WebApi.Handlers.GetMostCalmDates;
using WeatherStats.WebApi.Handlers.GetMostFrequentWindDirections;
using WeatherStats.WebApi.Handlers.GetMostFrequestWindDirectionOfStrongestTyphoons;
using WeatherStats.WebApi.Handlers.GetStrongestTyphoon;
using WeatherStats.WebApi.Handlers.Login;

/*
var data = CsvFileReader.ReadFile<TyphoonDataItem>("Files/typhoon_data.csv", startRowIndex: 1);

var stopWatch = Stopwatch.StartNew();
Parallel.ForEach(data, i => i.MaximumSustainedWindSpeed = i.MaximumSustainedWindSpeed + 100);
stopWatch.Stop();

Console.WriteLine("plinq: " + stopWatch.Elapsed.TotalMilliseconds);

stopWatch.Restart();
data.ForEach(i => i.MaximumSustainedWindSpeed = i.MaximumSustainedWindSpeed + 100);
stopWatch.Stop();

Console.WriteLine("linq: " + stopWatch.Elapsed.TotalMilliseconds);
*/

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                })
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "auth_cookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = false;

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddScoped<ITyphoonDataProvider, TyphoonDataProvider>(provider => 
    new TyphoonDataProvider("Files/typhoon_data.csv", "Files/typhoon_info.csv"));

builder.Services.AddScoped<GetAverageNumberOfTyphoonsHandler>();
builder.Services.AddScoped<GetAverageWindStrengthOfTyphoonsHandler>();
builder.Services.AddScoped<GetMostCalmDatesHandler>();
builder.Services.AddScoped<GetMostFrequentWindDirectionsHandler>();
builder.Services.AddScoped<GetMostFrequestWindDirectionOfStrongestTyphoonsHandler>();
builder.Services.AddScoped<GetStrongestTyphoonHandler>();
builder.Services.AddScoped<LoginHandler>();

builder.Services.AddHostedService<InitUsersService>();
builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
