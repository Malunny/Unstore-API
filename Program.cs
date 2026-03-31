using Microsoft.AspNetCore.Mvc;
using Unstorekle.Data;
using Unstorekle.Models;

using var context = new AppDbContext();
//  git commit -m "feat: Controllers dos models - Apenas gets funcionais até o momento"



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapControllers();

app.Run();