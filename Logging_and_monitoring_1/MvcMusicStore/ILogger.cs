using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcMusicStore
{
    public interface ILogger
    {
        void Log(string messageFormat, params object[] args);
        void LogDebug(string messageFormat, params object[] args);
        void LogError(string messageFormat, params object[] args);
    }
}
