using System;
using System.Linq;
using System.Threading;

namespace VeeamTest.Commons.Workers
{
    public class MultiThreadWorker<TRoutine> : WorkerBase<TRoutine>
        where TRoutine : IWorkerRepetitiveRoutine, IThreadSafe
    {
        private readonly int _degreeOfParallelism;

        private volatile Exception? _firstException;

        public MultiThreadWorker(TRoutine Routine, int DegreeOfParallelism) : base(Routine)
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
                base.Routine();
            }
            catch (Exception e)
            {
                Interlocked.CompareExchange(ref _firstException, e, null);
            }
        }

        protected override IterationResult OtherChecks()
        {
            return _firstException == null
                ? IterationResult.Continue
                : IterationResult.Break;
        }
    }
}