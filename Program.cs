using Microsoft.AspNetCore.Mvc;
using Unstore.Mapper;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;
using Unstore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

/*
using AppDbContext db = new();
DbDataSeeder.Seed(db);

var admin = db.Roles.First();

if (!await db.Users.Include(x => x.Role).AnyAsync(x => x.Role == admin))
{
    db.Users.Add(new User() 
    {
        FirstName = "Adm",
        LastName = "account",
        Username = "Administration",
        Email = "adm@mail.com",
        Password = "admin123",
        Role = admin,
        RoleId = admin.Id
    });
    db.SaveChanges();
}
*/
var builder = WebApplication.CreateBuilder(args);
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

// ------ SERVICES

builder.Services.AddTransient<TokenService>();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
    .ConfigureApiBehaviorOptions(options => {options.SuppressModelStateInvalidFilter = true;});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();