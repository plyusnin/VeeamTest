using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons.Workers
{
    public abstract class WorkerBase : IWorker
    {
        private readonly IProcessor _processor;
        private readonly IBlockSink _sink;
        private readonly IBlockSource _source;

        protected WorkerBase(IBlockSource Source, IBlockSink Sink, IProcessor Processor)
        {
            _processor = Processor;
            _sink      = Sink;
            _source    = Source;
        }

        public abstract void Run();

        protected void Routine()
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