using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced_CSharp_Types_1
{
    class Program
    {
        static void Main()
        {
            var s = new Source();
            var infoDataArray = new [] { 
                new InfoData(){ FirstName = "Vasya", LastName = "Vasyliev"},
                new InfoData(){ FirstName = "Vasya1", LastName = "Vasyliev1"},
                new InfoData(){ FirstName = "Vasya2", LastName = "Vasyliev2"},
                new InfoData(){ FirstName = "Vasya3", LastName = "Vasyliev3"},
                new InfoData(){ FirstName = "Vasya4", LastName = "Vasyliev4"},
            };

            s.CheckAndProceed(infoDataArray);
        }
    }
}
