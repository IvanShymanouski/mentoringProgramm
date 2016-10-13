using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SiteDownloader
{
    internal class TreeSearch
    {
        private readonly HtmlNode _rootNode;
        private readonly string _rootFolder;
        private readonly string _rootPath;
        private readonly string _protocol;
        private readonly int _depth;
        private readonly int _neededDepth;
        private List<ModernLink> _links;
        private readonly string[] _neededExt;
        private readonly ILogger _logger;

        public TreeSearch(HtmlNode rootNode, string rootFolder, string rootPath, string[] neededExt, int depth, int neededDepth, ILogger logger)
        {
            _rootNode = rootNode;
            _rootFolder = rootFolder;
            _rootPath = rootPath;
            _protocol = rootPath.Substring(0, rootPath.IndexOf('/'));
            _neededExt = neededExt;
            _depth = depth;
            _neededDepth = neededDepth;
            _logger = logger;
        }

        public List<ModernLink> Search()
        {
            _links = new List<ModernLink>(0);
            Search(_rootNode);
            return _links;
        }

        private void Search(HtmlNode parent)
        {
            foreach (var child in parent.ChildNodes)
            {
                HrefProcess(child.Attributes);
                SrcProcess(child.Attributes);
                Search(child);
            }
        }

        private void HrefProcess(HtmlAttributeCollection atrrs)
        {
            var hrefAttr = atrrs["href"];
            if (hrefAttr != null)
            {
                var link = hrefAttr.Value;

                var clearPath = ModernLink.ExtractClearUrl(link);
                var ending = GetEnding(clearPath);

                if (!string.IsNullOrEmpty(clearPath))
                {
                    if (ending == "ico" || ending == "js" || ending == "css" || ending == "png" || ending == "jpg" ||
                        ending == "gif")
                    {
                        if (_neededExt.Length == 0 || _neededExt.Contains(ending))
                        {
                            hrefAttr.Value = SaveSrc(clearPath);
                            return;
                        }
                    }
                }

                _logger?.LogWithLevel("link found " + link);
                if (!link.StartsWith("http"))
                {
                    if (link.StartsWith("//"))
                    {
                        link = _protocol + link;
                    }
                    else
                    {
                        link = _rootPath + link;
                    }
                }

                var newLink = new ModernLink(link);
                if (_depth + 1 <= _neededDepth)
                {
                    hrefAttr.Value = Path.Combine(newLink.Name, "index.html");
                    _links.Add(newLink);
                }
            }
        }

        private void SrcProcess(HtmlAttributeCollection atrrs)
        {
            var srcAttr = atrrs["src"];
            if (srcAttr != null)
            {
                var link = srcAttr.Value;
                var clearPath = ModernLink.ExtractClearUrl(link);
                if (!string.IsNullOrEmpty(clearPath))
                {
                    if (_neededExt.Length == 0 || _neededExt.Contains(GetEnding(clearPath)))
                    {
                        srcAttr.Value = SaveSrc(clearPath);
                    }
                }
            }
        }

        private string SaveSrc(string src)
        {
            _logger?.LogWithLevel("src found");
            var fileName = System.IO.Path.GetFileName(src);
            string fullName;

            do
            {
                fileName = "_" + fileName;
                fullName = System.IO.Path.Combine(_rootFolder, fileName);
            } while (System.IO.File.Exists(fullName));

            if (src.StartsWith("//"))
            {
                src = _protocol + src;
            }

            try
            {
                using (var c = new WebClient())
                    c.DownloadFile(src, fullName);
            }
            catch (WebException)
            {
                if (!src.StartsWith("http"))
                {
                    src = _rootPath + src;
                    try
                    {
                        using (var c = new WebClient())
                            c.DownloadFile(src, fullName);
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
                                _logger.LogWithLevel("File not found " + src);
                            }
                        }
                    }
                }
            }

            return fileName;
        }

        private static string GetEnding(string scr)
        {
            var pointI = scr.Length - 1;
            while (pointI > 0 && scr[pointI] != '.') pointI--;
            return scr.Substring(pointI + 1);
        }
    }
}
