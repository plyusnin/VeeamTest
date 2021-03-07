using System;
using System.Collections.Generic;
using System.IO;

namespace VeeamTest.Commons.FileManipulation
{
    public interface IBlockSource
    {
        public Block? Take();
    }

    public static class BlockSourceExtensions
    {
        public static IEnumerable<Block> TakeAll(this IBlockSource Reader)
        {
            while (true)
            {
                var block = Reader.Take();
                if (block != null)
                    yield return block;
                else
                    yield break;
            }
        }
    }

    public interface IBlockSourceFactory
    {
        public IBlockSource Create(Stream ForStream);
    }

    internal class LambdaBlockSourceFactory : IBlockSourceFactory
    {
        private readonly Func<Stream, IBlockSource> _delegate;

        public LambdaBlockSourceFactory(Func<Stream, IBlockSource> Delegate)
        {
            _delegate = Delegate;
        }

        public IBlockSource Create(Stream ForStream)
        {
            return _delegate(ForStream);
        }
    }
}