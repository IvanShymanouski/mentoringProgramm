using System;
using MSUtil;

namespace WorkWithLogParser
{
    public class LogManager
    {
        private static readonly COMTSVInputContextClass context = new COMTSVInputContextClass()
        {
            headerRow = false,
            iTsFormat = "yyyy-MM-dd HH:mm:ss,ffff",
            iSeparator = "|",
            nFields = 3
        };

        private readonly string _filename;
        public LogManager(string filename)
        {
            _filename = filename;
        }

        public ILogRecordset RowsCount
        {
            get
            {
                return new LogQueryClass().Execute(String.Format(@"SELECT Field2 AS LogType, COUNT(*) AS RowsCount FROM {0} GROUP BY Field2", _filename), context);
            }
        }

        public ILogRecordset Errors
        {
            get
            {
                return new LogQueryClass().Execute(String.Format(@"SELECT Field1 AS Date, Field2 AS LogType, Field3 AS Message FROM {0} WHERE Field2='ERROR'", _filename), context);
            }
        }
    }
}
