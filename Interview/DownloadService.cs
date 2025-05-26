using System.Text.Json;

namespace Interview
{
    public class DownloadService
    {
        private readonly HttpClient _httpClient = new();

        public async Task<List<string>> GetVersionsAsync(string url)
        {
            try
            {
                Console.WriteLine($"Starting download versions: {url}");
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"Finishing download versions: {response}");
                return JsonSerializer.Deserialize<List<string>>(response) ?? [];
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return [];
            }
        }

        public async Task<List<ManifestItem>> GetManifestByVersionAsync(string urlTemplate, string version)
        {
            string url = $"{urlTemplate}/{version}/index.json";

            Console.WriteLine($"Starting download manifest: {url}");

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"Finishing download manifest: {response}");

                return JsonSerializer.Deserialize<List<ManifestItem>>(response) ?? [];
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return [];
            }
        }

        public async Task DownloadResourceAsync(DownloadTaskInfo taskInfo)
        {
            var item = taskInfo.Item;
            string url = GetDownloadResourceUrl(item.path);
            string localPath = FilePathUtil.GetFilePath(item.path);

            try
            {
                string action = taskInfo.IsNew ? "New file" : "Overwrite";
                Console.WriteLine($"[Download]: {action} — {item.path}");


                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var bytes = await response.Content.ReadAsByteArrayAsync();

                Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);
                await File.WriteAllBytesAsync(localPath, bytes);

                Console.WriteLine($"[Download]: Completed — {item.path}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[Download]: Error downloading {item.path}: {exception.Message}");
            }
        }

        private string GetDownloadResourceUrl(string path)
        {
            return $"{Environment.GetEnvironmentVariable("BASE_URL")}{path}";
        }
    }
}