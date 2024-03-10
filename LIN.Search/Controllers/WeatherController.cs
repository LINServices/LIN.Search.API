using LIN.Types.Responses;

namespace LIN.Search.Controllers;


[Route("weather")]
public class WeatherController : ControllerBase
{

    /// <summary>
    /// Obtener el tiempo de una ciudad.
    /// </summary>
    /// <param name="city">Nombre de la ciudad.</param>
    [HttpGet]
    public async Task<HttpReadOneResponse<Weather>> GetWeather([FromQuery] string city)
    {

        // Validar parámetro.
        if (string.IsNullOrWhiteSpace(city))
            return new()
            {
                Message = "City no puede estar vacío.",
                Response = Types.Responses.Responses.InvalidParam,
            };

        // Obtener información.
        var weatherResponse = await LIN.Exp.Search.Client.GetWeatherAsync(city);

        // Validar.
        if (weatherResponse == null)
            return new()
            {
                Message = $"Hubo un error al obtener el tiempo de '{city}'",
                Response = Types.Responses.Responses.UnavailableService,
            };


        // Correcto.
        return new ReadOneResponse<Weather>
        {
            Model = weatherResponse,
            Response = Responses.Success
        };

    }


}