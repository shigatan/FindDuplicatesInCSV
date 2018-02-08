using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// base class of duplicates finder
    /// </summary>
    public abstract class DuplicatesFinder
    {
        protected readonly string csvFilePath;
        protected readonly int keyColumnIndex;
        private readonly IOutputWriter outputWriter;

        public DuplicatesFinder(string csvFilePath, int keyColumnIndex, IOutputWriter outputWriter)
        {
            this.csvFilePath = csvFilePath;
            this.keyColumnIndex = keyColumnIndex;
            this.outputWriter = outputWriter;
        }

        
        public abstract void FindDuplicates();

        public virtual void Print(string rowWithDplicatedColValue)
        {
            outputWriter.Print(rowWithDplicatedColValue);
        }

       
    }
}
