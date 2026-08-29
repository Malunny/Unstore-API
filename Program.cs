using Microsoft.AspNetCore.Mvc;
using Unstore.Data;
using Unstore.Services;
using Unstore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Unstore.DependencyInjection;
using Unstore.Services.Account;
using Unstore.Services.CommercialUser;
using Unstore.Services.Product;
using Unstore.Services.User;

var builder = WebApplication.CreateBuilder(args);

Configuration.JwtKey = builder.Configuration["JwtKey"]! as string;
Configuration.ApiKey = builder.Configuration["ApiKey"]! as string;

builder.Services.AddAuthenticationServices();
builder.Services.AddOpenApi();
builder.Services.AddMainServices();
builder.Services.AddDatabaseServices(builder, true);

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

