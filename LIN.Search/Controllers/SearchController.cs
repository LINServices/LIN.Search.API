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
                Response = Responses.InvalidParam,
            };

        // Cliente de Wikipedia.
        var wikiClient = new Wikipedia.WikipediaClient();

        // Solicitud.
        var wikiRequest = new Wikipedia.WikiSearchRequest(query)
        {
            WikiSearchMethod = Wikipedia.Enums.WikiSearchMethod.Text,
            Language = Wikipedia.Enums.WikiLanguage.Spanish,
            Limit = 1
        };

        // Enviar solicitud a Wikipedia.
        var wikiResponseTask = wikiClient.SearchAsync(wikiRequest);

        // Enviar solicitud a Bing.
        var searchResponseTask = LIN.Exp.Search.Client.Search(query);

        // Solicitud a LIN Maps.
        var placesResponseTask = LIN.Access.Maps.Controllers.Search.FindPlaces(query, 2, string.Empty, 5);

        // Esperar.
        var wiki = await wikiResponseTask;
        var search = await searchResponseTask;
        var places = await placesResponseTask;


        if (wiki?.QueryResult?.SearchResults.Count > 0)
        {
            var result = wiki?.QueryResult?.SearchResults[0];

            search.Insert(0, new()
            {
                Link = result!.Url.ToString(),
                ResultType = Types.Exp.Search.Enums.ResultType.Wikipedia,
                Title = result.Title,
                Snippet = result.Snippet ?? ""
            });
        }


        if (places.Models.Count > 0)
        {
            var result = places.Models[0];

            search.Insert(0, new()
            {
                ResultType = Types.Exp.Search.Enums.ResultType.Place,
                Title = result.Text,
                Snippet = result.Nombre
            });
        }


        // Validar.
        if (searchResponseTask == null)
            return new()
            {
                Message = $"Hubo un error al buscar '{query}'",
                Response = Responses.UnavailableService,
            };




        LIN.Access.OpenIA.IAModelBuilder builder = new();
        builder.Load(new Access.OpenIA.Models.Message()
        {
            Content = "Resume y completa con información que tengas, en un solo párrafo resume la información y completa.",
            Rol = Access.OpenIA.Models.Roles.System
        });


        string message = "";
        foreach(var e in search)
        {
            message += e.Snippet;
        }

        builder.Load(new Access.OpenIA.Models.Message() { Content = message, Rol = Access.OpenIA.Models.Roles.User });

        var reply = await builder.Reply("Resume con la información que se te ha dado y con la información que fue entrenado.");

        // Correcto.
        return new ReadAllResponse<SearchResult>
        {
            AlternativeObject = reply.Content,
            Models = search,
            Response = Responses.Success
        };

    }


}