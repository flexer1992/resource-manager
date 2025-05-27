namespace Interview
{
    public class App
    {
        private readonly VersionManager _versionManager;
        private readonly ResourceManager _resourceManager;
        private readonly ResourceManifestService _manifestService;

        public App(VersionManager versionManager, ResourceManager resourceManager,
            ResourceManifestService manifestService)
        {
            _versionManager = versionManager;
            _resourceManager = resourceManager;
            _manifestService = manifestService;
        }

        public async Task Run()
        {
            _resourceManager.SetupResourceFolders();
            string currentVersion = await _versionManager.GetTargetVersionAsync();

            List<DownloadTaskInfo> resourcesToDownload =
                await _manifestService.CollectResourcesToDownloadAsync(currentVersion, _versionManager.Versions);

            if (resourcesToDownload.Count > 0)
            {
                await _resourceManager.DownloadResourcesAsync(resourcesToDownload);
            }
            else
            {
                Console.WriteLine("No resource update required");
            }


            // Display 6 images
            var imagePaths = _manifestService.AppManifest.GetFilesPaths();

            Console.WriteLine("\nDisplaying 6 images:");
            for (int i = 0; i < 6; i++)
            {
                string path = i < imagePaths.Count ? imagePaths[i] : "placeholder.png";
                Console.WriteLine($"Image {i + 1}: {path}");
            }
        }
    }
}