using System;
using System.Linq;
using System.Threading;
using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons.Workers
{
    public class MultiThreadWorker : WorkerBase
    {
        private readonly int _degreeOfParallelism;

        private Exception? _firstException;

        public MultiThreadWorker(IBlockSource Source, IBlockSink Sink, IProcessor Processor, int DegreeOfParallelism)
            : base(Source, Sink, Processor)
        {
            _degreeOfParallelism = DegreeOfParallelism;
        }

        public override void Run()
        {
            var threads = Enumerable.Range(0, _degreeOfParallelism - 1)
                                    .Select(_ => new Thread(Routine))
                                    .ToList();

            foreach (var thread in threads)
                thread.Start();

            Routine();

            foreach (var thread in threads) thread.Join();

            if (_firstException != null)
                throw _firstException;
        }

        protected override void Routine()
        {
            try
            {
                while (true)
                    if (!ProcessDataPortion() || _firstException != null)
                        return;
            }
            catch (Exception e)
            {
                Interlocked.CompareExchange(ref _firstException, e, null);
            }
        }
    }
}