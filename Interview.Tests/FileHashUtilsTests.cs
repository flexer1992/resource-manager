using Xunit.Abstractions;

namespace Interview.Tests
{
    public class FileHashUtilsTests
    {
        [Fact]
        public async Task GetMd5_ShouldReturnCorrectHash()
        {
            var path = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(path, "hello world");
                var hash = await FileHashUtils.GetMd5FromFileAsync(path);
                Assert.Equal("5eb63bbbe01eeed093cb22bb8f5acdc3", hash);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async Task ComputeMd5Async_ShouldReturnIsNew_True_WhenFileDoesNotExist()
        {
            var item = new ManifestItem { path = "non_existing_file.txt", md5 = "irrelevant" };
            var result = await FileHashUtils.ComputeMd5Async(item);
            Assert.NotNull(result);
            Assert.True(result!.IsNew);
            Assert.Equal(item, result.Item);
        }
        
    }
}