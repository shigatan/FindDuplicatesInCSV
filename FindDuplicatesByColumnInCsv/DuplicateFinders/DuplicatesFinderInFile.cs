using System;
using System.Collections.Generic;
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
            var sortedHashMap = ReadFileDataToSortedHashMap(csvFilePath, keyColumnIndex);
            PrintOnlyDuplicates(sortedHashMap);
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
