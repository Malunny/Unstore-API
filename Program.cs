using Microsoft.AspNetCore.Mvc;
using Unstorekle.Data;
using Unstorekle.Models;

using var context = new AppDbContext();

var builder = WebApplication.CreateBuilder(args);
/*  BEING USED FOR TESTING PURPOSES
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  
            .AllowAnyHeader()  
            .AllowAnyMethod(); 
    });
});
*/
builder.Services
    .AddControllers()
    // ALSO BEING USED FOR TESTING PURPOSES
    //.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });;
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// REMOVE COMMENTS IF TESTING
// app.UseCors("AllowAll");
// app.UseAuthorization();

app.MapControllers();

app.Run();