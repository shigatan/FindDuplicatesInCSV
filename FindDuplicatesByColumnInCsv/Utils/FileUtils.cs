using DuplicatesInCSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// Utils to work with file
    /// </summary>
    public class FileUtils
    {
        public static double ConvertBytesToMegabytes(long bytes)
        {
            var totalInMb = (bytes / 1024f) / 1024f;
            return totalInMb;
        }

        public static bool isLargegFile(string filePath, int largeFileThreasholdInMb)
        {
            long fileSizeInBytes = new FileInfo(filePath).Length;
            return FileUtils.ConvertBytesToMegabytes(fileSizeInBytes) > largeFileThreasholdInMb;
        }

        public static OperationStatus FileIsValid(string path)
        {
            if (string.IsNullOrEmpty(path?.Trim()))
            {
                return new OperationStatus("Empty file name");
            }
            if (!File.Exists(path))
            {
                return new OperationStatus(string.Format("File [{0}] doesn't exist. Please re-run program with existed file path.", path));
            }
            // TODO: check extenstion

            return new OperationStatus();
        }

        internal static int FetchColumnCount(string csvFilePath)
        {
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string columnsStr = reader.ReadLine() ?? "";
                var columns = columnsStr.Split(',');
                return columns.IsNullOrEmpty() ? 0 : columns.Length;
            }
        }

        public static OperationStatus<int> FetchColumnIndex(string path, string columnName)
        {
            if (string.IsNullOrEmpty(columnName?.Trim()))
            {
                return new OperationStatus<int>("Empty file columnName");
            }
            try
            {
                var columnIndex = GetColumnIndex(path, columnName);
                if (columnIndex > -1)
                {
                    return new OperationStatus<int>(columnIndex);
                }
                else
                {
                    return new OperationStatus<int>("Column not found: " + columnName);
                }
            }
            catch (Exception ex)
            {
                return new OperationStatus<int>(string.Format("Column not found: {0}. {1}", columnName, ex));
            }
        }

        private static int GetColumnIndex(string filePath, string column)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string columnsStr = reader.ReadLine() ?? "";
                var columns = columnsStr.Split(',');
                return columns.IsNullOrEmpty() ? -1 : Array.IndexOf(columns, column);
            }
        }

        public static void CreateFileWithFakeData(string filePath, int rows = 5000000)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            int lastIdx = chars.Length - 1;
            var random = new Random();
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("column1,column2");
                long i = 0;
                while (i < rows)
                {
                    string str = string.Format("{0},{1}", random.Next(0, 10), chars[random.Next(0, lastIdx)]);
                    file.WriteLine(str);
                    i++;
                }
            }
        }
    }
}
