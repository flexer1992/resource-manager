using System.Text.Json;

namespace Interview
{
    public class ResourceManifestService
    {
        private AppManifest? _appManifest;

        private readonly DownloadService _downloadService;
        private readonly ResourceManager _resourceManager;

        public ResourceManifestService(DownloadService downloadService, ResourceManager resourceManager)
        {
            _downloadService = downloadService;
            _resourceManager = resourceManager;
        }

        public async Task<List<DownloadTaskInfo>> CollectResourcesToDownloadAsync(string appVersion,
            List<string> appVersions)
        {
            // проверим наличие манифеста
            if (!CheckExistAndLoadManifest())
            {
                // если нет, то создадим пустой с нужной версии сразу
                BuildManifestForVersion(appVersion);
            }

            int versionIndex = appVersions.IndexOf(appVersion);


            for (int i = 0; i <= versionIndex; i++)
            {
                string version = appVersions[i];

                // если нет локально манифеста, то скачиваем его
                if (!IsExistManifestByAppVersion(version))
                {
                    var manifestItems =
                        await _downloadService.GetManifestByVersionAsync(Environment.GetEnvironmentVariable("BASE_URL"),
                            version);

                    SaveManifestFileByVersion(manifestItems, version);

                    List<string>? removeResourcePaths = _appManifest?.Merge(manifestItems);
                    _resourceManager.RemoveResourcesByPath(removeResourcePaths);
                    SaveManifest();
                }
            }

            // проверяем на диске и считаем хеш сумму
            List<DownloadTaskInfo> itemsToDownload =
                await FileHashUtils.ComputeMd5ForFilesAsync(_appManifest?.Resources);


            if (itemsToDownload.Count > 0)
            {
                Console.WriteLine("Resources for update/download:");    
            }
            
            foreach (var resource in itemsToDownload)
            {
                string message = resource.IsNew ? "download" : "update";
                Console.WriteLine($"resource {resource.Item.id}: need be {message}");
            }

            return itemsToDownload;
        }

        private bool CheckExistAndLoadManifest()
        {
            if (File.Exists(Constants.ManifestPath))
            {
                var json = File.ReadAllText(Constants.ManifestPath);
                _appManifest = JsonSerializer.Deserialize<AppManifest>(json);
                Console.WriteLine("[AppManifest]: Manifest is exist.");
                return true;
            }

            Console.WriteLine("[AppManifest]: Manifest is not exist.");
            return false;
        }

        private void BuildManifestForVersion(string appVersion)
        {
            _appManifest = new AppManifest
            {
                AppVersion = appVersion,
                Resources = []
            };

            Console.WriteLine($"[AppManifest]: Build manifest for app version:{appVersion}");
        }

        private void SaveManifest()
        {
            File.Create(Constants.ManifestPath).Close();
            File.WriteAllText(Constants.ManifestPath, JsonSerializer.Serialize(_appManifest));

            Console.WriteLine("[AppManifest]: Save manifest file");
        }

        private bool IsExistManifestByAppVersion(string appVersion)
        {
            return File.Exists(Constants.ManifestsPath + $"/{appVersion}.json");
        }

        private void SaveManifestFileByVersion(List<ManifestItem> manifestItems, string appVersion)
        {
            string path = Constants.ManifestsPath + $"/{appVersion}.json";

            File.Create(path).Close();
            File.WriteAllText(path, JsonSerializer.Serialize(manifestItems));

            Console.WriteLine($"[AppManifest]: Save manifest file for app version:{appVersion}");
        }
    }
}