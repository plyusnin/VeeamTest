using System;
using System.IO;

namespace VeeamTest.Commons.FileManipulation
{
    public interface IBlockSink
    {
        public void Put(Block Block);
    } 

    public interface IBlockSinkFactory
    {
        public IBlockSink Create(Stream ForStream);
    }

    internal class LambdaBlockSinkFactory : IBlockSinkFactory
    {
        private readonly Func<Stream, IBlockSink> _delegate;

        public LambdaBlockSinkFactory(Func<Stream, IBlockSink> Delegate)
        {
            _delegate = Delegate;
        }

        public IBlockSink Create(Stream ForStream)
        {
            return _delegate(ForStream);
        }
    }
}