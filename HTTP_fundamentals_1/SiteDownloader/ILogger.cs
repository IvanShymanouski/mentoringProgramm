namespace SiteDownloader
{
    public interface ILogger
    {
        int DeepLevel { get; set; }
        void Log(string formatString, params object[] ps);
        void LogWithLevel(string formatString, params object[] ps);
    }
}
