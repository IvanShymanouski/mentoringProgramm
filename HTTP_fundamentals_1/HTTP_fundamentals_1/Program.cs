using System;
using SiteDownloader;

namespace HTTP_fundamentals_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var url1 = "http://ru.stackoverflow.com/questions/476984/%D0%9A%D0%B0%D0%BA-%D1%81%D0%BE%D1%85%D1%80%D0%B0%D0%BD%D0%B8%D1%82%D1%8C-%D0%B8%D0%B7%D0%BE%D0%B1%D1%80%D0%B0%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F-%D1%81-%D1%81%D0%B0%D0%B9%D1%82%D0%B0-%D0%BD%D0%B0-c#comment546274_476984";
            var url2 = "http://www.google.com";
            var url = url2;
            var downloader = new FixedLevelDownloader(url, 1, @"e:\", /*new string[]{"js","css"},*/ new Logger());

            Console.WriteLine("-----Doanloading is started------");

            downloader.Download();

            Console.WriteLine("-----Doanloading is finished------");
            Console.WriteLine("Press any key...");

            Console.ReadKey();
        }
    }
}
