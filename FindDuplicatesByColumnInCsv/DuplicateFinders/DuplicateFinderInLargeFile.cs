using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// hanlde search duplicates in large file
    /// </summary>
    public class DuplicatesFinderInLargeFile : DuplicatesFinder
    {

        //const int ChunkSizeInMb = 100 * 1024;
        //const int ReadBufferSizeInBytes = 10 * 1024;

        private readonly int columnCount;

        const int ChunkSizeInBytes = 5 * 1024 * 1024;  // can be configured
        private int ReadBufferSizeInBytes;
        const int RowCountToWrite = 5000;
                
        public DuplicatesFinderInLargeFile(string csvFilePath, int keyColumnIndex, IOutputWriter writer) : base(csvFilePath, keyColumnIndex, writer)
        {
            columnCount = FileUtils.FetchColumnCount(csvFilePath);            
        }

        public override void FindDuplicates()
        {
            Console.WriteLine("Split into chunks");
            var chunks = SplitFileIntoChunks(csvFilePath, ChunkSizeInBytes);
            if (chunks.IsNullOrEmpty())
            {
                Console.WriteLine("File wasn't splitted");
                return;
            }
            Console.WriteLine("Sorted chunks separately");
            var sortedChunks = SortEachChunk(chunks, keyColumnIndex);
            Console.WriteLine("MergeChunks");
            ReadBufferSizeInBytes = (int) ChunkSizeInBytes / chunks.Count();
            MergeChunks(sortedChunks.ToArray());
        }

        private string BuildChunkTempFileName(string filePath, int chunkIndex)
        {
            return string.Format("{0}_chunk_{1}_temp.csv", Path.GetFileNameWithoutExtension(filePath), chunkIndex);            
        }

        private string BuildSortedChunkTempFileName(string tempFilePath)
        {
            return string.Format("{0}_sorted.csv", Path.GetFileNameWithoutExtension(tempFilePath));
        }

        private string[] SplitFileIntoChunks(string filePath, int chunkSizeInMb)
        {
            List<string> chunkFiles = new List<string>();
            int chunkIdx = 0;
            using (var fileStream = File.OpenRead(filePath))
            {
                byte[] chunkBytes = new byte[chunkSizeInMb];
                while (fileStream.Read(chunkBytes, 0, chunkBytes.Length) > 0)
                {
                    var chunkFileName = BuildChunkTempFileName(filePath, chunkIdx++);
                    using (var fs = new FileStream(chunkFileName, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(chunkBytes, 0, chunkBytes.Length);
                    }
                    chunkFiles.Add(chunkFileName);
                }
            }
            return chunkFiles.ToArray();
        }

        private IEnumerable<string> SortEachChunk(string[] chunks, int keyColumnIndex)
        {
            List<string> sortedChunkFilePaths = new List<string>();
            for(int i= 0; i < chunks.Length; i++)
            {
                bool isFirstChunk = i == 0;
                sortedChunkFilePaths.Add(SortChunk(chunks[i], keyColumnIndex, isFirstChunk));
                File.Delete(chunks[i]);
            }
            return sortedChunkFilePaths;
        }

        private string SortChunk(string chunkFilePath, int keyColumnIndex, bool isFirstChunk)
        {
            var sortedData = ReadFileDataToSortedHashMap(chunkFilePath, keyColumnIndex, isFirstChunk);
            string sortedChunkFileName = BuildSortedChunkTempFileName(chunkFilePath);
            WriteSortedDataToFile(sortedData, sortedChunkFileName);
            return sortedChunkFileName;
        }


        private void WriteSortedDataToFile(IDictionary<string, List<string>> hashMap, string sortedChunkFileName)
        {
            using (StreamWriter sortedChunkFile = new StreamWriter(sortedChunkFileName))
            {
                StringBuilder sb = new StringBuilder();
                int sbLinesCounter = 0;
                foreach (var columnLines in hashMap)
                {
                    foreach (var line in columnLines.Value)
                    {
                        sb.Append(columnLines.Key);
                        sb.Append(",");
                        sb.AppendLine(line);
                        sbLinesCounter++;
                    }
                    if (sbLinesCounter > RowCountToWrite)
                    {
                        sortedChunkFile.Write(sb.ToString());
                        sb.Clear();
                        sbLinesCounter = 0;
                    }
                }
                if (sbLinesCounter > 0)
                {
                    sortedChunkFile.Write(sb.ToString());
                }
            }
        }

        private IList<QueueChunkFileReader> CreateChunkReaders(string[] sortedChunks)
        {
            List<QueueChunkFileReader> chunkReaders = new List<QueueChunkFileReader>();
            for (int i = 0; i < sortedChunks.Count(); i++)
            {
                chunkReaders.Add(new QueueChunkFileReader(sortedChunks[i], ReadBufferSizeInBytes, columnCount));
            }
            return chunkReaders;            
        }

        private void MergeChunks(string[] sortedChunks)
        {
            var chunkReaders = CreateChunkReaders(sortedChunks);
            chunkReaders = chunkReaders.OrderBy(x => x.GetCurrentKeyColumnValue()).ToList();
            var printer = new DuplicatesPrinter((str)=> Print(str));
            while (chunkReaders.Count > 0)
            {
                var keyColumnValue = chunkReaders[0].GetCurrentKeyColumnValue();
                foreach (var chunkReader in chunkReaders)
                {
                    if (!chunkReader.IsEnd)
                    {
                        List<string> values;
                        bool isDataFinished = false;
                        while (!isDataFinished)
                        {
                            isDataFinished = chunkReader.TryGetValuesStartedWith(keyColumnValue, out values);
                            if (values.IsNullOrEmpty())
                                break;

                            printer.Print(keyColumnValue.Length + 1, values);
                        }
                    }
                    else
                    {
                        chunkReader.Dispose();
                    }
                }
                chunkReaders = chunkReaders.Where(x => !x.IsEnd).OrderBy(x => x.GetCurrentKeyColumnValue()).ToList();
            }
            CloseReaders(chunkReaders);
            RemoveFiles(sortedChunks);
        }

        private void CloseReaders(IList<QueueChunkFileReader> chunkReaders)
        {
            foreach(var item in chunkReaders)
            {
                item.Dispose();
            }
        }
        
        private void RemoveFiles(string[] chunkFiles)
        {
            foreach (var file in chunkFiles)
            {
                try
                {
                    Task.Delay(5000).Wait();
                    File.Delete(file);
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
