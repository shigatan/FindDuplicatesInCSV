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

        protected IDictionary<string, List<string>> ReadFileDataToSortedHashMap(string filePath, int keyColumnIndex, bool toMissHeader = true)
        {
            var sortedHashMap = new SortedDictionary<string, List<string>>();

            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                if (toMissHeader)
                {
                    line = sr.ReadLine();
                    toMissHeader = false;
                }
                while ((line = sr.ReadLine()) != null)
                {
                    var columnValue = line.Split(',').SafeGetByIndex(keyColumnIndex);
                    if (!string.IsNullOrEmpty(columnValue))
                    {
                        sortedHashMap.AddOrUpdate(columnValue, line);
                    }
                }
            }
            return sortedHashMap;
        }
    }
}
