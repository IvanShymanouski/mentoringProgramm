using log4net;
using System.Collections.Generic;

namespace MvcMusicStore
{
    /// <summary>Simplify the logging/printing of test-case messages</summary>
    public class Logger : ILogger
    {
        private readonly ILog infoLogger = LogManager.GetLogger("Info");
        private readonly ILog debugLogger = LogManager.GetLogger("Debug");
        private readonly ILog errorLogger = LogManager.GetLogger("Error");

        /// <summary>Log the given format/arguments to the Console</summary>
        /// <param name="messageFormat">A format string</param>
        /// <param name="args">Optional arguments to the format string</param>
        public void Log(string messageFormat, params object[] args)
        {
            try
            {
                infoLogger.InfoFormat(messageFormat, args);
            }
            catch (System.FormatException)
            {
                infoLogger.Info("Loggig error : String contains special characters.");
            }
        }

        public void LogDebug(string messageFormat, params object[] args)
        {
            try
            {
                debugLogger.InfoFormat(messageFormat, args);
            }
            catch (System.FormatException)
            {
                debugLogger.Info("Loggig error : String contains special characters.");
            }
        }

        public void LogError(string messageFormat, params object[] args)
        {
            try
            {
                errorLogger.ErrorFormat(messageFormat, args);
            }
            catch (System.FormatException)
            {
                errorLogger.Error("Loggig error : String contains special characters.");
            }
        }
    }
}