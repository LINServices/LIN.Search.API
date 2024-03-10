global using Http.ResponsesList;
global using LIN.Search;
global using LIN.Types.Exp.Search.Models;
global using Microsoft.AspNetCore.Mvc;
global using LIN.Types.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Establecer llave.
LIN.Exp.Search.Client.SetWeatherApi(Configuration.GetConfiguration("weather"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
