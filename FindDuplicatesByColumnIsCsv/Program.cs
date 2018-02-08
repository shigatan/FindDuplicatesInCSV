using DuplicatesInCSV;
using System;
namespace FindDuplicatesByColumnIsCsv
{
    class Program
    {
        /// <summary>
        /// App allows to find rows from csv with duplicate Values in specified column 
        /// </summary>
        /// <param name="args"></param>
        
        static void Main(string[] args)
        {
            if (args.Length < 2 )
            {
                Console.WriteLine("Please run program with input parameters: PathToCsvFile columnName");
                Console.ReadLine();
                return;
            }
            string filePath = args[0];
            string columnName = args[1];

            //the main idea is sort values by specified column and then output only rows with duplicated values
            var duplicatesFinder = new DuplicatesFinderFacade();
            duplicatesFinder.Run(filePath, columnName);
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }       
    }
}
