using LIN.Types.Responses;

namespace LIN.Search.Controllers;


[Route("search")]
public class SearchController : ControllerBase
{

    /// <summary>
    /// Obtener resultados de búsqueda.
    /// </summary>
    /// <param name="query">Consulta.</param>
    [HttpGet]
    public async Task<HttpReadAllResponse<SearchResult>> Get([FromQuery] string query)
    {

        // Validar parámetro.
        if (string.IsNullOrWhiteSpace(query))
            return new()
            {
                Message = "La consulta no puede estar vacío.",
                Response = Types.Responses.Responses.InvalidParam,
            };

        // Obtener información.
        var searchResponse = await LIN.Exp.Search.Client.Search(query);

        // Validar.
        if (searchResponse == null)
            return new()
            {
                Message = $"Hubo un error al buscar '{query}'",
                Response = Types.Responses.Responses.UnavailableService,
            };


        // Correcto.
        return new ReadAllResponse<SearchResult>
        {
            Models = searchResponse,
            Response = Responses.Success
        };

    }


}