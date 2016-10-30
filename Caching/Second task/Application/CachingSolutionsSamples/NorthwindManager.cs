using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
	public class NorthwindManager
    {
		private INorthwindCache cache;

		public NorthwindManager(INorthwindCache cache)
		{
			this.cache = cache;
		}

		public IEnumerable<T> GetEntities<T>() where T : class
		{
			Console.WriteLine($"Get {typeof(T).Name}");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var entities = cache.Get<T>(user);

			if (entities == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					entities = dbContext.Set<T>().ToList();
					cache.Set(user, entities);
				}
			}

			return entities;
		}
	}
}
