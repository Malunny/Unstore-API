using Microsoft.AspNetCore.Mvc;
using Unstore.Mapper;
using Unstore.Data;
using Unstore.Services;
using Unstore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Unstore.Models;
using Unstore.Services.Account;

var builder = WebApplication.CreateBuilder(args);

ConfigureKeysAndTokens();
AddAuthentication();

AddServices();
ConfigureDbContext();

var app = builder.Build();  

app.UseCors(policy => policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


void AddServices()
{
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<AccountService>();
    builder.Services.AddSingleton<IServiceResultFactoryProvider, DataServiceResultFactoryProvider>();
    builder.Services.AddTransient<ITokenService, JwtTokenService>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddMemoryCache();
    builder.Services
        .AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
        .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
}
void ConfigureKeysAndTokens()
{
    Configuration.JwtKey = builder.Configuration["Jwt-Key"]! as string;
    Configuration.ApiKey = builder.Configuration["Api-Key"]! as string;
}
void ConfigureDbContext()
{
    builder.Services.AddDbContext<AppDbContext>();
}
void AddAuthentication()
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(options =>
    {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}
