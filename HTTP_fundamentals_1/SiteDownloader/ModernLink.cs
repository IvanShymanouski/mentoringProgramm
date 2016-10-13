using System;
using System.Net;

namespace SiteDownloader
{
    internal class ModernLink
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        private const int PathNameLength = 50;

        public ModernLink(string path)
        {
            Name = MakeDirectoryNameFromUrl(path);
            Path = path;
        }

        public static string MakeDirectoryNameFromUrl(string url)
        {
            var neededUrlName = System.Web.HttpUtility.HtmlEncode(url)
                                                     .Replace(".", "_")
                                                     .Replace(@"/", "")
                                                     .Replace(":", "_")
                                                     .Replace("?", "_")
                                                     .Replace("|","")
                                                     .Replace("&", "");
            return neededUrlName.Substring(neededUrlName.Length - PathNameLength > 0 ? neededUrlName.Length - PathNameLength : 0);
        }

        public static string ExtractClearUrl(string url)
        {
            var iOfQ = url.IndexOf('?');
            if (iOfQ > 0)
            {
                url = url.Substring(0, iOfQ);
            }
            var iOfD = url.Length - 1;
            while (iOfD >= url.Length && url[iOfD] == '/') iOfD--;
            return url.Substring(0, iOfD + 1);
        }
    }
}
