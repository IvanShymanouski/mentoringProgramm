using CustomParsers;
using System;

namespace Exceptions_handling_2
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] values = { null,  "+13230", "-0", "1,390,146", "$190,235,421,127",
                          "0xFA1B", "163042", "-10", "2147483647", 
                          "2147483648", "16e07", "134985.0", "-12034",
                          "-2147483648", "-2147483649" };
            foreach (string value in values)
            {
                try
                {
                    int number = value.ParseCustom();
                    Console.WriteLine("{0} --> {1}", value, number);
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Attempted conversion of '<null>' failed.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0}: Bad Format", value);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("{0}: Overflow", value);
                }
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("======  TryParse  =====");
            Console.WriteLine(Environment.NewLine);

            values = new string[] {null,  "160519", "9432.0", "16,667", "   -322   ", "+4302", "(100);", "01FA" };
            foreach (var value in values)
            {
                int number;

                bool result = value.TryParseCustom(out number);
                if (result)
                {
                    Console.WriteLine("Converted '{0}' to {1}.", value, number);
                }
                else
                {
                    //            if (value == null) value = ""; 
                    Console.WriteLine("Attempted conversion of '{0}' failed.",
                                       value == null ? "<null>" : value);
                } 
            }

            Console.ReadKey();
        }
    }
}
