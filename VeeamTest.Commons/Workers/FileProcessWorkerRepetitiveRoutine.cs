using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons.Workers
{
    /// <summary>Репитетивная операция по обработке файла указанным <see cref="IProcessor" /></summary>
    public class FileProcessWorkerRepetitiveRoutine : IWorkerRepetitiveRoutine
    {
        private readonly IProcessor _processor;
        private readonly IBlockSink _sink;
        private readonly IBlockSource _source;

        public FileProcessWorkerRepetitiveRoutine(IBlockSource Source, IBlockSink Sink, IProcessor Processor)
        {
            _processor = Processor;
            _sink      = Sink;
            _source    = Source;
        }

        public IterationResult Iterate()
        {
            var block = _source.Take();
            if (block == null) return IterationResult.Break;

            var processedBlock = _processor.Process(block);
            _sink.Put(processedBlock);

            return IterationResult.Continue;
        }

        public override string ToString()
        {
            return $"{_source} ~~({_processor})~~> {_sink}";
        }
    }
}