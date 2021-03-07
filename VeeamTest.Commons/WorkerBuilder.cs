using System;
using System.IO;
using VeeamTest.Commons.FileManipulation;
using VeeamTest.Commons.FileManipulation.Lego;
using VeeamTest.Commons.FileManipulation.Lockable;
using VeeamTest.Commons.FileManipulation.Plain;
using VeeamTest.Commons.Processing;
using VeeamTest.Commons.Workers;

namespace VeeamTest.Commons
{
    public class WorkerBuilder
    {
        public WorkerBuilder(IBlockSourceFactory SourceFactory, IBlockSinkFactory SinkFactory, IProcessor Processor)
        {
            this.SourceFactory  = SourceFactory;
            this.SinkFactory    = SinkFactory;
            this.Processor      = Processor;
            DegreeOfParallelism = 1;
        }

        public IBlockSourceFactory SourceFactory       { get; }
        public IBlockSinkFactory   SinkFactory         { get; }
        public IProcessor          Processor           { get; }
        public int                 DegreeOfParallelism { get; private set; }

        public static WorkerBuilder Compressor(int BlockSize)
        {
            return new(PlainBlockSource.Factory(BlockSize),
                       LegoBlockSink.Factory,
                       new GzipPackProcessor());
        }

        public static WorkerBuilder Decompressor()
        {
            return new(LegoBlockSource.Factory,
                       PlainBlockSink.Factory,
                       new GzipPackProcessor());
        }

        public WorkerBuilder WithDegreeOfParallelism(int ThreadsCount)
        {
            DegreeOfParallelism = ThreadsCount;
            return this;
        }

        public WorkerBuilder Parallel()
        {
            DegreeOfParallelism = Environment.ProcessorCount;
            return this;
        }


        public IWorker BuildFor(Stream Input, Stream Output)
        {
            return DegreeOfParallelism switch
            {
                1 => new SingleThreadWorker(
                    new FileProcessWorkerRepetitiveRoutine(
                        SourceFactory.Create(Input),
                        SinkFactory.Create(Output),
                        Processor)),

                _ => new MultiThreadWorker(
                    new FileProcessWorkerRepetitiveRoutine(
                        SourceFactory.Create(Input).Locked(),
                        SinkFactory.Create(Output).Locked(),
                        Processor),
                    DegreeOfParallelism)
            };
        }
    }
}