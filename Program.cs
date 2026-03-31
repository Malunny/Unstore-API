using Microsoft.AspNetCore.Mvc;
using Unstorekle.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapControllers();

app.Run();