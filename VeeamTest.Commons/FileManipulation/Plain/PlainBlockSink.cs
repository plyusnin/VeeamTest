using System.IO;

namespace VeeamTest.Commons.FileManipulation.Plain
{
    public class PlainBlockSink : IBlockSink
    {
        private readonly Stream _stream;

        public PlainBlockSink(Stream Stream)
        {
            _stream = Stream;
        }

        public static IBlockSinkFactory Factory => new LambdaBlockSinkFactory(s => new PlainBlockSink(s));

        public void Put(Block Block)
        {
            _stream.Seek(Block.Offset, SeekOrigin.Begin);
            _stream.Write(Block.Data, 0, Block.Data.Length);
        }
    }
}