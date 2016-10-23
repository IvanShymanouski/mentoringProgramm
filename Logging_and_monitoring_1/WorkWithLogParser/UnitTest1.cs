using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSUtil;

namespace WorkWithLogParser
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var resultDataSet = new LogManager("e:\\Log.txt").RowsCount;

            while (!resultDataSet.atEnd())
            {
                var record = resultDataSet.getRecord();
                Console.WriteLine(String.Format("{0} - {1}", record.getValue("LogType"), record.getValue("RowsCount")));
                resultDataSet.moveNext();
            }

            resultDataSet = new LogManager("e:\\Log.txt").Errors;
            while (!resultDataSet.atEnd())
            {
                var record = resultDataSet.getRecord();
                Console.WriteLine(String.Format("{0} - {1} - {2}", record.getValue("Date"), record.getValue("LogType"), record.getValue("Message")));
                resultDataSet.moveNext();
            }
        }
    }
}
