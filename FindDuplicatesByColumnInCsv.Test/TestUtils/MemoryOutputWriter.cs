using DuplicatesInCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicatesByColumnInCsv
{
    internal class MemoryOutputWriter : IOutputWriter
    {
        public List<string> Output { get; private set; }

        public MemoryOutputWriter()
        {
            Output = new List<string>();
        }

        public void Print(string str)
        {
            Output.Add(str);
        }
    }
}
