using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    class DuplicatesFinderFactory
    {
        /// <summary>
        /// create duplicate finder depends on size of file
        /// if file has small size this task can be solved without concerns about memory using Hash Map => O(n)
        /// if file has large size it can not be fitted to memory and task can be solved using external merge sort algorithm.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static DuplicatesFinder Create(string filePath, int columnIndex)
        {
            const int LargeFileSizeThreasholdInMb = 25 * 1024; // can be configured

            bool isLargeFile = FileUtils.isLargegFile(filePath, LargeFileSizeThreasholdInMb);
            DuplicatesFinder duplicatesFinder = null;
            if (isLargeFile)
            {
                // create DuplicatesFinderInLargeFile implemented external merge sort
                duplicatesFinder = new DuplicatesFinderInLargeFile(filePath, columnIndex, new ConsoleOutputWriter());
            }
            else
            {
                // create class implemented sorting using Sorted Hash Map
                duplicatesFinder = new DuplicatesFinderInFile(filePath, columnIndex, new ConsoleOutputWriter());
            }
            return duplicatesFinder;
        }
    }
}
