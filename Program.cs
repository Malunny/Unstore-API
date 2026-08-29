using Microsoft.AspNetCore.Mvc;
using Unstore.Data;
using Unstore.Services;
using Unstore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Unstore.Services.Account;
using Unstore.Services.CommercialUser;
using Unstore.Services.Product;
using Unstore.Services.User;

var builder = WebApplication.CreateBuilder(args);

ConfigureKeysAndTokens();
AddAuthentication();
builder.Services.AddOpenApi();
AddServices();
string? dbConnectionString = builder.Configuration.GetConnectionString("UnstoredbCloud");
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(dbConnectionString);
});

var app = builder.Build();

app.UseCors(policy => policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    Configuration.TokenExpirationTimeHours = 96;
    app.MapOpenApi();
    app.UseSwaggerUI(options => {options.SwaggerEndpoint("/openapi/v1.json", "Unstore API v1");});    
}
else
    app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();

app.Run();


void AddServices()
{
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<AccountService>();
    builder.Services.AddScoped<CommercialAccountService>();
    builder.Services.AddScoped<CommercialUserActionService>();
    builder.Services.AddScoped<UserVerificationService>();
    builder.Services.AddScoped<UserPurchaseService>();
    builder.Services.AddSingleton<IServiceResultFactory, DataServiceResultFactory>();
    builder.Services.AddTransient<ITokenService, JwtTokenService>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddMemoryCache();
    
    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        })
        .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
}
void ConfigureKeysAndTokens()
{
    Configuration.JwtKey = builder.Configuration["JwtKey"]! as string;
    // Configuration.ApiKey = builder.Configuration["Api-Key"]! as string;
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
