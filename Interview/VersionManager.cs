namespace Interview
{
    public class VersionManager
    {
        private readonly string _currentVersion;
        private readonly DownloadService _downloadService;
        private List<string> _versions;
        public List<string> Versions => _versions;

        public VersionManager(DownloadService downloadService)
        {
            _downloadService = downloadService;
            _currentVersion = Environment.GetEnvironmentVariable("APP_VERSION") ?? "v1";
        }

        public async Task<string> GetTargetVersionAsync()
        {
            string versionUrl = Environment.GetEnvironmentVariable("BASE_URL") + Constants.MANIFEST_PATH;

            _versions = await _downloadService.GetVersionsAsync(versionUrl);

            if (!_versions.Any())
            {
                throw new Exception("No versions in download list");
            }

            if (!_versions.Contains(_currentVersion))
            {
                throw new Exception($"Not found {_currentVersion} in download list");
            }

            // todo доп сортировка, не нужна если с сервера гарантируется правильный порядок версий
            _versions = _versions
                .OrderBy(v => int.Parse(v.Substring(1))) // сортируем по числу после 'v'
                .ToList();

            Console.WriteLine($"Current version: {_currentVersion}");

            return _currentVersion;
        }
    }
}