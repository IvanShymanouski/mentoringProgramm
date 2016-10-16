using MvcMusicStore.Infrastructure;
using PerformanceCounterHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore
{
    public static class Counter
    {
       public static CounterHelper<Counters> Instanse { get; private set; }
        static Counter()
        {
            Instanse = PerformanceHelper.CreateCounterHelper<Counters>("Mentoring");
        }
    }
}