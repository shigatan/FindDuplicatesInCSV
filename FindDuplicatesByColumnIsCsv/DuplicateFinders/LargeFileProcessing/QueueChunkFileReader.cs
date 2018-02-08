using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// allows to read chunk from file and set data to queue    
    /// </summary>
    internal class QueueChunkFileReader : ChunkFileReader, IDisposable
    {
        Queue<string> queue;
        private readonly int columnCount;
        private string previousUnhandledPart = null;

        public QueueChunkFileReader(string filePath, int chunkReadSize, int columnCount)
            : base(filePath, chunkReadSize)
        {
            this.columnCount = columnCount + 1; // + key column
        }

        public void GrabNextChunkToQueue()
        {
            if (IsEnd)
                return;

            if (queue == null)
                queue = new Queue<string>();

            if (queue.Count > 0)
                return;


            var data = ReadChunkInBytes();
            if (data != null)
            {
                UTF8Encoding temp = new UTF8Encoding(true);
                var tempString = temp.GetString(data);
                var lines = tempString.Split('\n');
                foreach (var line in lines)
                {
                    var current = line.TrimEnd();
                    if (!string.IsNullOrEmpty(previousUnhandledPart))
                    {
                        current = previousUnhandledPart + line;
                        previousUnhandledPart = null;
                    }
                    var values = current.Split(',');
                    if (values.Count() < this.columnCount || string.IsNullOrEmpty(values[values.Count() -1]))
                    {
                        previousUnhandledPart = current;
                    }
                    else if (!string.IsNullOrEmpty(current))
                    {
                        queue.Enqueue(current);
                    }
                }
            }
        }

        public bool TryGetValuesStartedWith(string keyColumnValue, out List<string> rowsWithSameKeyColumnValue)
        {
            bool dataIsFinished = false;
            rowsWithSameKeyColumnValue = new List<string>();

            if (IsEnd)
                return true;

            if (queue.Count == 0)
            {
                GrabNextChunkToQueue();
            }


            while (!IsEnd)
            {
                if (queue?.Count == 0)
                {
                    dataIsFinished = true;
                    break;
                }
                else if (queue.Peek().StartsWith(keyColumnValue))
                {
                    rowsWithSameKeyColumnValue.Add(queue.Dequeue());
                }
                else
                {
                    break;
                }
            }
            return dataIsFinished;
        }

        public string GetCurrentKeyColumnValue()
        {
            GrabNextChunkToQueue();
            return (IsEnd || queue == null || queue.Count == 0) ? null : queue.Peek().Split(',').SafeGetByIndex(0);
        }
    }
}
