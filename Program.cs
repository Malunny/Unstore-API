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

var builder = WebApplication.CreateBuilder(args);

ConfigureKeysAndTokens();
AddAuthentication();

AddServices(builder);
ConfigureDbContext();

var app = builder.Build();  

app.UseCors(policy => policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


void AddServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<AccountService>();
    builder.Services.AddScoped<ClientService>();
    builder.Services.AddScoped<EmployeeService>();
    builder.Services.AddScoped<PositionService>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<RoleService>();
    builder.Services.AddScoped<ServiceService>();
    builder.Services.AddScoped<ToolService>();
    builder.Services.AddScoped<ToolTagService>();
    builder.Services.AddTransient<TokenService>();
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
