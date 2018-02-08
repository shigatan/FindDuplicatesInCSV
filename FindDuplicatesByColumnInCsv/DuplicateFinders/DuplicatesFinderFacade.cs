using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    public class DuplicatesFinderFacade
    {
        public OperationStatus Run(string filePath, string columnName)
        {
            // validate file name
            
            var isValid = FileUtils.FileIsValid(filePath);
            if (!isValid.IsSuccess)
            {
                Console.WriteLine(isValid.ErrorMessage);
                return isValid;
            }

            // validate column name
            var columnIndex = FileUtils.FetchColumnIndex(filePath, columnName);
            if (!columnIndex.IsSuccess)
            {
                Console.WriteLine(columnIndex.ErrorMessage); 
                return columnIndex;
            }

            // find duplicates
            DuplicatesFinder duplicatesFinder = DuplicatesFinderFactory.Create(filePath, columnIndex.Result);
            duplicatesFinder.FindDuplicates();
            Console.WriteLine("Finished");
            return new OperationStatus();
        }
    }
}
