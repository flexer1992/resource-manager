namespace Interview.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async Task App_Run_ShouldDownloadAndPurgeAssets_Correctly()
        {
            // Настраиваем переменные окружения
            Environment.SetEnvironmentVariable("BASE_URL", "https://geo.karateka-team.ru"); 
            // Говорим приложению, что мы «текущая версия» v1, 
            Environment.SetEnvironmentVariable("APP_VERSION", "v1");

            // Удалим любые остатки папки resources из старых тестов
            var resourcesPath = Path.Combine(AppContext.BaseDirectory, "resources");
            if (Directory.Exists(resourcesPath))
                Directory.Delete(resourcesPath, recursive: true);

            // Создаём инстансы сервисов и запускаем App.Run()
            var downloadService = new DownloadService();
            var versionManager = new VersionManager(downloadService);
            var resourceManager = new ResourceManager(downloadService);
            var manifestService = new ResourceManifestService(downloadService, resourceManager);

            var app = new App(versionManager, resourceManager, manifestService);
            await app.Run();

            var assetsDir = Path.Combine(AppContext.BaseDirectory, "resources", "assets");
            Assert.True(Directory.Exists(assetsDir), "Должна была появиться папка с ресурсами");

            var files = Directory.GetFiles(assetsDir).Select(Path.GetFileName).ToList();

            Assert.Equal(6, files.Count);

            Assert.Contains("ab.png", files);
            Assert.Contains("bb.png", files);
            Assert.Contains("cb.png", files);
            Assert.Contains("db.png", files);
            Assert.Contains("eb.png", files);
            Assert.Contains("fb.png", files);


            if (Directory.Exists(resourcesPath))
                Directory.Delete(resourcesPath, recursive: true);
        }
    }
}