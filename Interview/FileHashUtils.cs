using System.Security.Cryptography;

namespace Interview
{
    public class FileHashUtils
    {
        public static async Task<string> GetMd5FromFileAsync(string filePath)
        {
            using var md5 = MD5.Create();
            await using var stream = File.OpenRead(filePath);
            var hash = await Task.Run(() => md5.ComputeHash(stream));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public static async Task<DownloadTaskInfo?> ComputeMd5Async(ManifestItem manifestItem)
        {
            try
            {
                string filePath = FilePathUtil.GetFilePath(manifestItem.path);

                if (!File.Exists(filePath))
                {
                    return new DownloadTaskInfo { Item = manifestItem, IsNew = true };
                }


                var fileHash = await GetMd5FromFileAsync(filePath);

                if (!fileHash.Equals(manifestItem.md5))
                {
                    return new DownloadTaskInfo { Item = manifestItem, IsNew = false };
                }
            }
            catch
            {
                return new DownloadTaskInfo { Item = manifestItem, IsNew = false };
            }

            return null;
        }

        public static async Task<List<DownloadTaskInfo?>> ComputeMd5ForFilesAsync(List<ManifestItem> manifestItems)
        {
            var tasks = manifestItems.Select(ComputeMd5Async).ToArray();
            var results = await Task.WhenAll(tasks);
            return results.Where(item => item != null).ToList();
        }
    }
}