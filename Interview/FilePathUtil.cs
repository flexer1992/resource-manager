namespace Interview
{
    public class FilePathUtil
    {
        public static string GetFilePath(string filePath)
        {
            return $"{Constants.ResourcesPath}{filePath}";
        }
    }
}