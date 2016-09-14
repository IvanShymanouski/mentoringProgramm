using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced_CSharp_Types_1
{
    class Source
    {
        internal void CheckAndProceed(IInfoData[] data)
        {
            var dest = new Destination();

            //do something

            dest.ProceedData(data);
        }
    }
}
