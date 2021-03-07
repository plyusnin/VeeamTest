using System.Linq;
using System.Threading;
using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons.Workers
{
    public class MultiThreadWorker : WorkerBase
    {
        private readonly int _degreeOfParallelism;

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
        }
    }
}