using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    ///  allows to read only chunk from file
    /// </summary>
    internal class ChunkFileReader : IDisposable
    {
        private readonly int chunkReadSize;
        private readonly string filePath;
       
        private FileStream streamReader;

        public ChunkFileReader(string filePath, int chunkReadSize)
        {
            this.chunkReadSize = chunkReadSize;
            this.filePath = filePath;            
        }

        public bool IsEnd { get; private set; }

        public byte[] ReadChunkInBytes()
        {
            if (streamReader == null)
            {
                streamReader = File.OpenRead(filePath);
            }

            byte[] bChunk = new byte[this.chunkReadSize];
            if (streamReader.Read(bChunk, 0, bChunk.Length) > 0)
            {
                return bChunk;
            }
            else
            {
                IsEnd = true;
            }
            return null;
        }

        private bool disposed = false;

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (this.streamReader != null)
                {
                    this.streamReader.Close();
                    this.streamReader.Dispose();
                }
                disposed = true;
            }
        }
    }
}
