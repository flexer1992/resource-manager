namespace Interview
{
    public class Constants
    {
        public static readonly string ResourcesPath = Path.Combine(AppContext.BaseDirectory, "resources");
        public static readonly string AssetsPath = Path.Combine(ResourcesPath, "assets");
        public static readonly string ManifestPath = Path.Combine(ResourcesPath, "manifest.json");
        public static readonly string ManifestsPath = Path.Combine(ResourcesPath, "manifests");
        public const string MANIFEST_PATH = "/versions.json";
    }
}