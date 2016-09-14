using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced_CSharp_Types_1
{
    class Destination
    {
        internal void ProceedData(IInfoData[] data)
        {
            for(var i = 0; i < data.Length; i++)
            {
                //do something with data[i];
            }
        }
    }
}
