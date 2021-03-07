using System.IO;

namespace VeeamTest.Commons.FileManipulation.Lego
{
    public class LegoBlockSource : IBlockSource
    {
        private readonly BinaryReader _reader;

        public LegoBlockSource(Stream Stream)
        {
            _reader = new BinaryReader(Stream);
        }

        public static IBlockSourceFactory Factory => new LambdaBlockSourceFactory(s => new LegoBlockSource(s));

        public Block? Take()
        {
            if (_reader.BaseStream.Position == _reader.BaseStream.Length)
                return null;

            var offset = _reader.ReadInt64();
            var length = _reader.ReadInt32();
            var data   = _reader.ReadBytes(length);
            return new Block(offset, data);
        }
    }
}