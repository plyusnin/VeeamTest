using System.Linq;
using System.Threading;
using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons
{
    public class Worker
    {
        private readonly IProcessor _processor;
        private readonly IBlockSink _sink;
        private readonly IBlockSource _source;
        private readonly int _threadsCount = 6;

        public Worker(IBlockSource Source, IBlockSink Sink, IProcessor Processor)
        {
            _processor = Processor;
            _sink      = Sink;
            _source    = Source;
        }

        public void Run()
        {
            var threads = Enumerable.Range(0, _threadsCount)
                                    .Select(_ => new Thread(Routine))
                                    .ToList();

            foreach (var thread in threads) thread.Start();
            
            // TODO: Как-то надо дождаться, пока все потоки закончатся
        }

        private void Routine()
        {
            while (true)
            {
                var block = _source.Take();
                if (block == null) return;

                var processedBlock = _processor.Process(block);
                _sink.Put(processedBlock);
            }
        }
    }
}