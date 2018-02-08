using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    public interface IOutputWriter
    {
        void Print(string str);
    }
}
