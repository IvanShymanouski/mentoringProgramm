using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;
using System.Data.SqlClient;

namespace CachingSolutionsSamples
{
	internal class NorthwindMemoryCache : INorthwindCache, IDisposable
	{
		private const string DefaultPrefix  = "Cache_";
		ObjectCache cache = MemoryCache.Default;
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

        public NorthwindMemoryCache()
        {
            SqlDependency.Start(_connectionString);
        }

		public IEnumerable<T> Get<T>(string forUser)
		{
			return (IEnumerable<T>) cache.Get(GetKey<T>(forUser));
		}

		public void Set<T>(string forUser, IEnumerable<T> entities)
        {
            var policy = new CacheItemPolicy();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SELECT [CategoryID] FROM [dbo].[Categories]", conn))
                {
                    command.CommandTimeout = Int32.MaxValue;
                    var dependency = new SqlDependency(command);
                    conn.Open();
                    command.ExecuteNonQuery();
                    policy.ChangeMonitors.Add(new SqlChangeMonitor(dependency));
                }
            }

            cache.Set(GetKey<T>(forUser), entities, policy);
        }

        private string GetKey<T>(string forUser)
        {
            return DefaultPrefix + typeof(T).Name + forUser;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SqlDependency.Stop(_connectionString);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
