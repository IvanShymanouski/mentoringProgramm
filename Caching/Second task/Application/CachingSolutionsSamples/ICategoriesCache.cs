using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
	public interface INorthwindCache
	{
		IEnumerable<T> Get<T>(string forUser);

		void Set<T>(string forUser, IEnumerable<T> entities);
	}
}
