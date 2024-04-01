namespace LIN.Search.Controllers;


[Route("movies")]
public class MoviesController : ControllerBase
{

   
    [HttpGet]
    public async Task<HttpReadOneResponse<Movie>> GetMovie([FromQuery] string movie)
    {

        // Validar parámetro.
        if (string.IsNullOrWhiteSpace(movie))
            return new()
            {
                Message = "City no puede estar vacío.",
                Response = Responses.InvalidParam,
            };

        // Obtener información.
      var response = await LIN.Exp.Search.Client.SearchMovie(movie);

    
        // Correcto.
        return new ReadOneResponse<Movie>
        {
            Model= response,
            Response = Responses.Success
        };

    }


}