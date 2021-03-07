using System.IO;

namespace VeeamTest.Commons.FileManipulation.Lego
{
    public class LegoBlockSink : IBlockSink
    {
        private readonly BinaryWriter _writer;

        public LegoBlockSink(Stream Stream)
        {
            _writer = new BinaryWriter(Stream);
        }

        public static IBlockSinkFactory Factory => new LambdaBlockSinkFactory(s => new LegoBlockSink(s));

        public void Put(Block Block)
        {
            _writer.Write(Block.Offset);
            _writer.Write(Block.Data.Length);
            _writer.Write(Block.Data);
        }
    }
}