using System;
using System.IO;

namespace VeeamTest.Commons.FileManipulation.Plain
{
    public class PlainBlockSource : IBlockSource
    {
        private readonly int _blockSize;
        private readonly Stream _stream;

        public PlainBlockSource(Stream Stream, int BlockSize)
        {
            _stream    = Stream;
            _blockSize = BlockSize;
        }

        public Block? Take()
        {
            if (_stream.Position == _stream.Length)
                return null;

            var buffer = new byte[_blockSize];
            var offset = _stream.Position;
            var len    = _stream.Read(buffer, 0, _blockSize);
            if (len == _blockSize) return new Block(offset, buffer);

            var newBuffer = new byte[len];
            Buffer.BlockCopy(buffer, 0, newBuffer, 0, len);
            return new Block(offset, newBuffer);
        }

        public static IBlockSourceFactory Factory(int BlockSize)
        {
            return new LambdaBlockSourceFactory(s => new PlainBlockSource(s, BlockSize));
        }
    }
}