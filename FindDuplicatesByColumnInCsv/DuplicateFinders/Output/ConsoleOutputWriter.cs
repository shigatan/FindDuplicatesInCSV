using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    class ConsoleOutputWriter : IOutputWriter
    {
        public void Print(string str)
        {
            Console.WriteLine(str);
        }
    }
}
