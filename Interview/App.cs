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
            
        }
    }
}