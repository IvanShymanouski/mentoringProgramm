using System;
using System.IO;
using HtmlAgilityPack;
using System.Net;

namespace SiteDownloader
{
    public class FixedLevelDownloader
    {
        private readonly string _siteStoragePath;
        private readonly string _startUrl;
        private readonly int _neededDeepLevel;
        private readonly string[] _neededExt;

        private const int MaxDeepLevel = 5;

        private readonly ILogger _logger;

        #region ctrs
        public FixedLevelDownloader(string startUrl, int neededDeepLevel, string siteStoragePath)
        {
            _neededExt = new string[0];
            _startUrl = startUrl;
            _neededDeepLevel = (MaxDeepLevel < neededDeepLevel) ? MaxDeepLevel : neededDeepLevel;
            _siteStoragePath = siteStoragePath;
        }

        public FixedLevelDownloader(string startUrl, int neededDeepLevel, string siteStoragePath, string[] takenExt)
            : this(startUrl, neededDeepLevel, siteStoragePath)
        {
            _neededExt = takenExt;
        }

        public FixedLevelDownloader(string startUrl, int neededDeepLevel, string siteStoragePath, ILogger logger)
            : this(startUrl, neededDeepLevel, siteStoragePath)
        {
            _logger = logger;
        }

        public FixedLevelDownloader(string startUrl, int neededDeepLevel, string siteStoragePath, string[] takenExt, ILogger logger)
            : this(startUrl, neededDeepLevel, siteStoragePath, takenExt)
        {
            _logger = logger;
        }
        #endregion

        public void Download()
        {
            Download(new ModernLink(_startUrl), 0, _siteStoragePath);
        }

        private void Download(ModernLink neededUrl, int deepLevel, string rootFoulder)
        {
            if (deepLevel > _neededDeepLevel)
            {
                return;
            }

            string pathString = System.IO.Path.Combine(rootFoulder, neededUrl.Name);
            if (!System.IO.Directory.Exists(pathString))
            {
                System.IO.Directory.CreateDirectory(pathString);
            }
            rootFoulder = pathString;

            WebClient client = new WebClient();
            var document = new HtmlDocument();
            try
            {
                document.Load(client.OpenRead(neededUrl.Path), System.Text.Encoding.UTF8);
            }
            catch (WebException ex)
            {
                if (_logger != null)
                {
                    if (!ex.Message.Contains("404"))
                    {
                        _logger.LogWithLevel("****ERROR****");
                        _logger.LogWithLevel(ex.Message);
                        _logger.LogWithLevel("****ERROR****");
                    }
                    else
                    {
                        _logger.LogWithLevel("Page not found " + neededUrl.Path);
                    }
                }
            }

            if (_logger != null)
            {
                _logger.DeepLevel = deepLevel;
                _logger.Log(Environment.NewLine);
                _logger.Log(" ***************** == Deep level {0} == ***************** ", _logger.DeepLevel);
                _logger.Log(Environment.NewLine);
            }

            var linkList = new TreeSearch(document.DocumentNode, pathString, ModernLink.ExtractClearUrl(neededUrl.Path), _neededExt, deepLevel, _neededDeepLevel, _logger).Search();
            document.Save(Path.Combine(pathString, "index.html"));

            _logger?.LogWithLevel("Links:");
            foreach (var link in linkList)
            {
                _logger?.LogWithLevel(link.Path);
                Download(link, deepLevel + 1, pathString);
            }
        }
    }
}
