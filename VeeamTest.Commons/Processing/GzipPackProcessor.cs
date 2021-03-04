using System.IO;
using System.IO.Compression;

namespace VeeamTest.Commons.Processing
{
    public class GzipPackProcessor : IProcessor
    {
        public byte[] Process(byte[] Input, int Offset, int Count)
        {
            var inputStream  = new MemoryStream(Input, Offset, Count);
            var outputStream = new MemoryStream();
            using (var zipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                inputStream.CopyTo(zipStream);
            }

            return outputStream.ToArray();
        }
    }
}