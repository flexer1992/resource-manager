// See https://aka.ms/new-console-template for more information

using DotNetEnv;
using Interview;

internal class Program
{
    private static App app;
        
    static async Task Main(string[] args)
    {
        Console.WriteLine("Application started.");
        Env.Load();
        DownloadService downloadService = new DownloadService();
        VersionManager versionManager = new VersionManager(downloadService);
        ResourceManager resourceManager = new ResourceManager(downloadService);
        ResourceManifestService manifestService = new ResourceManifestService(downloadService, resourceManager);

        app = new App(
            versionManager,
            resourceManager,
            manifestService
        );

        try
        {
            await app.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Application terminated. : {e.Message}");
        }
        
        Console.WriteLine("Application finished.");
    }
}