using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests // нужно запустить redis-server.exe из папки с пакетами, чтобы работало
    {
		[TestMethod]
		public void MemoryCache()
		{
            using (var cache = new NorthwindMemoryCache())
            {
                var categoryManager = new NorthwindManager(cache);
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine(categoryManager.GetEntities<Category>().Count());
                    if (4 < i && i <= 5)
                    {
                        using (var db = new Northwind())
                        {
                            db.Set<Category>().Add(new Category() { CategoryName = "4", Description = "Описание4" });
                            db.SaveChanges();
                        } 
                    }
                    Thread.Sleep(100);
                } 
            }
        }

		[TestMethod]
		public void RedisCache()
		{
			var categoryManager = new NorthwindManager(new NorthwindRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetEntities<Category>().Count());
				Thread.Sleep(100);
			}
		}
	}
}
