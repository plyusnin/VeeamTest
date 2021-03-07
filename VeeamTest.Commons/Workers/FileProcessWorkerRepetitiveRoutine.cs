using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.Processing;

namespace VeeamTest.Commons.Workers
{
    public static class FileProcessWorkerRepetitiveRoutine
    {
        public static FileProcessWorkerRepetitiveRoutine<TSource, TSink, TProcessor> Create<TSource, TSink, TProcessor>(
            TSource Source, TSink Sink, TProcessor Processor)
            where TSource : IBlockSource
            where TSink : IBlockSink
            where TProcessor : IProcessor
        {
            return new(Source, Sink, Processor);
        }

        public static ThreadSafeFileProcessWorkerRepetitiveRoutine<TSource, TSink, TProcessor> CreateThreadSafe<TSource,
            TSink,
            TProcessor>(TSource Source, TSink Sink, TProcessor Processor)
            where TSource : IBlockSource, IThreadSafe
            where TSink : IBlockSink, IThreadSafe
            where TProcessor : IProcessor, IThreadSafe
        {
            return new(
                Source, Sink, Processor);
        }
    }

    public class FileProcessWorkerRepetitiveRoutine<TSource, TSink, TProcessor> : IWorkerRepetitiveRoutine
        where TSource : IBlockSource
        where TSink : IBlockSink
        where TProcessor : IProcessor
    {
        private readonly IProcessor _processor;
        private readonly IBlockSink _sink;
        private readonly IBlockSource _source;

        public FileProcessWorkerRepetitiveRoutine(TSource Source, TSink Sink, TProcessor Processor)
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

    public class ThreadSafeFileProcessWorkerRepetitiveRoutine<TSource, TSink, TProcessor>
        : FileProcessWorkerRepetitiveRoutine<TSource, TSink, TProcessor>, IThreadSafe
        where TSource : IBlockSource, IThreadSafe
        where TSink : IBlockSink, IThreadSafe
        where TProcessor : IProcessor, IThreadSafe
    {
        public ThreadSafeFileProcessWorkerRepetitiveRoutine(TSource Source, TSink Sink, TProcessor Processor)
            : base(Source, Sink, Processor) { }
    }
}