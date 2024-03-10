namespace LIN.Search;


public class Configuration
{


    /// <summary>
    /// Servicio.
    /// </summary>
    private static IConfiguration? Config;


    /// <summary>
    /// Si el servicio esta iniciado.
    /// </summary>
    private static bool _isStart = false;



    /// <summary>
    /// Obtener una configuración.
    /// </summary>
    /// <param name="route">Ruta.</param>
    public static string GetConfiguration(string route)
    {

        if (_isStart && Config != null)
            return Config[route] ?? string.Empty;

        var configBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false, true);

        Config = configBuilder.Build();
        _isStart = true;

        return Config[route] ?? string.Empty;

    }


}