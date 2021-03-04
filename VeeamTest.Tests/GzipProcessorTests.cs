using System;
using System.Linq;
using NUnit.Framework;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Tests
{
    public class GzipProcessorTests
    {
        private GzipPackProcessor _packer;
        private byte[] _testData;
        private GzipUnpackProcessor _unpacker;

        [SetUp]
        public void Setup()
        {
            var r = new Random();
            _testData = Enumerable
                       .Range(0, 456)
                       .Select(_ => Enumerable.Repeat((byte) r.Next(0x00, 0xff),
                                                      r.Next(1, 255)))
                       .SelectMany(x => x)
                       .ToArray();

            _packer   = new GzipPackProcessor();
            _unpacker = new GzipUnpackProcessor();
        }

        [Test]
        public void ForthAndBackBulk()
        {
            var zippedData   = _packer.Process(_testData);
            var unzippedData = _unpacker.Process(zippedData);

            Assert.That(unzippedData, Is.EqualTo(_testData));
        }
    }
}