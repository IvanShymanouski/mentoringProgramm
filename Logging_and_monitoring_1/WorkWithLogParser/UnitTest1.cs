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
            ILogRecordset rsLP = null;
            ILogRecord rowLP = null;

            double UsedBW = 0;
            int Unitsprocessed;

            string strSQL = null;

            var LogParser = new LogQueryClass();
            var W3Clog = new COMFileSystemInputContextClass();

            try
            {
                //W3C Logparsing SQL. Replace this SQL query with whatever 
                //you want to retrieve. The example below 
                //will sum up all the bandwidth
                //Usage of a specific folder with name 
                //"userID". Download Log Parser 2.2 
                //from Microsoft and see sample queries.

                strSQL = @"SELECT * from e:\\InfoLog.txt";

                // run the query against W3C log
                rsLP = LogParser.Execute(strSQL, W3Clog);

                rowLP = rsLP.getRecord();

                Unitsprocessed = rsLP.inputUnitsProcessed;

                if (rowLP.getValue(0).ToString() == "0" ||
                    rowLP.getValue(0).ToString() == "")
                {
                    //Return 0 if an err occured
                    UsedBW = 0;
                    Console.WriteLine(UsedBW);
                }

                //Bytes to MB Conversion
                double Bytes = Convert.ToDouble(rowLP.getValue(0).ToString());
                UsedBW = Bytes / (1024 * 1024);

                //Round to 3 decimal places
                UsedBW = Math.Round(UsedBW, 3);
            }
            catch(Exception ex)
            {
                throw;
            }

            Console.WriteLine(UsedBW);
        }
    }
}
