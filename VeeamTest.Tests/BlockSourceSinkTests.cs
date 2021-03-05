using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.FileManipulation.Lego;
using VeeamTest.Commons.FileManipulation.Plain;

namespace VeeamTest.Tests
{
    public class BlockSourceSinkTests
    {
        private byte[] _testData;

        [SetUp]
        public void Setup()
        {
            var r = new Random();
            _testData = new byte[4567];
            r.NextBytes(_testData);
        }

        [Test]
        public void PlainParserForthAndBack()
        {
            const int blockSize = 500;

            var input  = new MemoryStream(_testData);
            var output = new MemoryStream();

            var sink   = new PlainBlockSink(output);
            var source = new PlainBlockSource(input, blockSize);

            PassForthAndBack(source, sink);

            Assert.That(output.ToArray(), Is.EqualTo(_testData));
        }

        [Test]
        public void LegoParserCrossCheck()
        {
            const int blockSize = 500;

            var rawStream = new MemoryStream(_testData);
            var output    = new MemoryStream();

            var plainSource = new PlainBlockSource(rawStream, blockSize);
            var blocks      = Randomize(plainSource.TakeAll()).ToList();

            var legoStream = new MemoryStream();
            var legoSink   = new LegoBlockSink(legoStream);

            legoStream.Seek(0, SeekOrigin.Begin);
            foreach (var block in blocks) legoSink.Put(block);

            legoStream.Seek(0, SeekOrigin.Begin);
            var legoSource = new LegoBlockSource(legoStream);

            var afterLegoBlocks = legoSource.TakeAll().ToList();
            var plainSink       = new PlainBlockSink(output);
            foreach (var block in afterLegoBlocks) plainSink.Put(block);

            Assert.That(output.ToArray(), Is.EqualTo(_testData));
        }

        private void PassForthAndBack(PlainBlockSource Source, PlainBlockSink Sink)
        {
            var blocks           = Source.TakeAll().ToList();
            var randomizedBlocks = Randomize(blocks);

            foreach (var block in randomizedBlocks) Sink.Put(block);
        }

        private IEnumerable<T> Randomize<T>(IEnumerable<T> Input)
        {
            var r = new Random();
            return Input.Select(x => new { payload = x, order = r.NextDouble() })
                        .OrderBy(x => x.order)
                        .Select(x => x.payload);
        }
    }
}