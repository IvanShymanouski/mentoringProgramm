using System;
using System.Text;

namespace SiteDownloader
{
    public class Logger : ILogger
    {

        public int DeepLevel
        {
            get { return _deepLevel; }
            set
            {
                _deepLevel = value;
                StringBuilder dBuilder = new StringBuilder();
                for (var i = 0; i < _deepLevel; i++)
                {
                    dBuilder.Append("-");
                }
                _dashes = dBuilder + " ";
            }
        }

        private int _deepLevel = 0;
        private string _dashes = "";

        public void Log(string formatString, params object[] ps)
        {
            Console.WriteLine(formatString, ps);
        }
        public void LogWithLevel(string formatString, params object[] ps)
        {
            Console.WriteLine(_dashes + formatString, ps);
        }
    }
}
