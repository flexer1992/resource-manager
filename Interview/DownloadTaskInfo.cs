namespace Interview
{
    public class DownloadTaskInfo
    {
        public ManifestItem Item { get; set; }
        public bool IsNew { get; set; } // true — файл отсутствовал, false — был, но повреждён или надо обновить
    }
}