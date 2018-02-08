using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// search duplicates in non-large file
    /// </summary>
    public class DuplicatesFinderInFile : DuplicatesFinder
    {
        public DuplicatesFinderInFile(string csvFilePath, int keyColumnIndex, IOutputWriter writer) 
            : base(csvFilePath, keyColumnIndex, writer)
        {
        }

        public override void FindDuplicates()
        {
            var sortedHashMap = ReadFileDataToHashMap(csvFilePath, keyColumnIndex);
            PrintOnlyDuplicates(sortedHashMap);
        }


        private IDictionary<string, List<string>> ReadFileDataToHashMap(string filePath, int keyColumnIndex, bool toMissHeader = true)
        {
            var sortedHashMap = new Dictionary<string, List<string>>();

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

        private void PrintOnlyDuplicates(IDictionary<string, List<string>> sortedHashMapBySelectedColumn)
        {
            if (sortedHashMapBySelectedColumn.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in sortedHashMapBySelectedColumn)
            {
                if (item.Value.Count > 1)
                {
                    item.Value.ForEach((line) => Print(line));
                }
            }
        }

    }
}
