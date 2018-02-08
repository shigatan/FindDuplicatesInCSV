using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    internal class DuplicatesPrinter
    {
        private string _previous = null;
        private int _count = 0;
        private Action<string> _printAction;

        public DuplicatesPrinter(Action<string> printAction)
        {
            _printAction = printAction;
        }
        
        public void Print(int skipLength, List<string> values)
        {
            foreach (var line in values)
            {
                var printedString = line.Substring(skipLength);
                if (_previous == null)
                {
                    _previous = printedString;
                    _count = 1;
                    continue;
                }

                if (_previous == printedString)
                {
                    if (_count == 1)
                    {
                        Print(_previous);
                    }
                    Print(printedString);
                    _count++;
                }
                else
                {
                    _previous = printedString;
                }
            }
        }

        public void Print(string str)
        {
            Console.WriteLine(str);
        }
    }
}
