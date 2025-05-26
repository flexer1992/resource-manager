namespace Interview
{
    public class ResourceManager
    {
        private readonly DownloadService _downloadService;

        public ResourceManager(DownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        // создаем при отсутсвии папки для ресурсов
        public void SetupResourceFolders()
        {
            Console.WriteLine("[Resources]: Setup directories started");

            CheckAndCreateFolder(Constants.ResourcesPath, nameof(Constants.ResourcesPath));
            CheckAndCreateFolder(Constants.ManifestsPath, nameof(Constants.ManifestsPath));
            CheckAndCreateFolder(Constants.AssetsPath, nameof(Constants.AssetsPath));

            Console.WriteLine("[Resources]: Setup directories finished");
        }


        private void CheckAndCreateFolder(string folderPath, string folderName)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"[Resources]: Create {folderName} directory");
                Directory.CreateDirectory(folderPath);
            }
        }

        public async Task DownloadResourcesAsync(List<DownloadTaskInfo> manifestItems)
        {
            Console.WriteLine("[Resources]: Start loading");

            var tasks = manifestItems.Select(_downloadService.DownloadResourceAsync).ToList();
            await Task.WhenAll(tasks);

            Console.WriteLine("[Resources]: Finish loading");
        }

        public void RemoveResourcesByPath(List<string>? paths)
        {
            foreach (var path in paths)
            {
                string filePath = FilePathUtil.GetFilePath(path);

                if (!File.Exists(filePath))
                {
                    continue;
                }

                File.Delete(filePath);
                Console.WriteLine($"[Resources]: File {filePath} deleted");
            }
        }
    }
}