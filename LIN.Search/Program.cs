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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Establecer llave.
LIN.Exp.Search.Client.SetWeatherApi(Configuration.GetConfiguration("weather"));
LIN.Exp.Search.Client.SetMoviesApi(Configuration.GetConfiguration("imbd"));

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
