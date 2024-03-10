global using Microsoft.AspNetCore.Mvc;
global using LIN.Search;
global using LIN.Types.Exp.Search.Models;
global using LIN.Types.Exp.Search.Enums;
global using Http.ResponsesList;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

// Establecer llave.
LIN.Exp.Search.Client.SetWeatherApi(Configuration.GetConfiguration("weather"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
