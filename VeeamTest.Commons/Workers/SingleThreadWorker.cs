using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons.Workers
{
    public class SingleThreadWorker : WorkerBase
    {
        public SingleThreadWorker(IBlockSource Source, IBlockSink Sink, IProcessor Processor)
            : base(Source, Sink, Processor) { }

        public override void Run()
        {
            Routine();
        }
    }
}