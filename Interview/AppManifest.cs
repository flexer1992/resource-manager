namespace Interview
{
    public class AppManifest
    {
        public string AppVersion { get; set; }

        public List<ManifestItem> Resources { get; set; }

        public List<string>? Merge(List<ManifestItem> manifestItems)
        {
            var resourceMap = Resources.ToDictionary(item => item.id);
            var removedPaths = new List<string>();

            foreach (var newItem in manifestItems)
            {
                if (resourceMap.TryGetValue(newItem.id, out var existingItem))
                {
                    if (!string.Equals(existingItem.md5, newItem.md5, StringComparison.OrdinalIgnoreCase))
                    {
                        resourceMap[newItem.id] = newItem;
                        removedPaths.Add(existingItem.path);
                        Console.WriteLine($"[Manifest]: resource {newItem.id} - needs update!");
                    }
                }
                else
                {
                    resourceMap[newItem.id] = newItem;
                    Console.WriteLine($"[Manifest]: Resource {newItem.id} - is new!");
                }
            }

            Resources = resourceMap.Values.ToList();
            return removedPaths;
        }

        public List<string> GetFilesPaths()
        {
            return Resources.Select(item => $"{Constants.AssetsPath}" + item.path).ToList();
        }
    }
}