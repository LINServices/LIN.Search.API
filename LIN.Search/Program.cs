global using Http.ResponsesList;
global using LIN.Search;
global using LIN.Types.Exp.Search.Models;
global using Microsoft.AspNetCore.Mvc;
global using LIN.Types.Responses;
global using Http.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLINHttp();

var app = builder.Build();

// Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Establecer llave.
LIN.Exp.Search.Client.SetWeatherApi(Configuration.GetConfiguration("weather"));
LIN.Exp.Search.Client.SetMoviesApi(Configuration.GetConfiguration("imbd"));
LIN.Access.OpenIA.OpenIA.SetKey(Configuration.GetConfiguration("openIA"));

app.UseHttpsRedirection();

app.UseLINHttp();

app.UseAuthorization();

app.MapControllers();

app.Run();