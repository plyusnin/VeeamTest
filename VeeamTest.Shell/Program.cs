using System;
using System.Diagnostics;
using System.IO;
using VeeamTest.Commons;

namespace VeeamTest.Shell
{
    internal class Program
    {
        private static readonly int _blockSize = 1024 * 1024;

        private static void Main(string[] args)
        {
            if (args.Length < 3)
                throw new Exception("Число параметров должно быть равно 3");

            var operation      = args[0];
            var inputFileName  = args[1];
            var outputFileName = args[2];

            using var inputFile  = File.OpenRead(inputFileName);
            using var outputFile = File.Create(outputFileName);

            var workerBuilder = operation switch
            {
                "compress"   => WorkerBuilder.Compressor(_blockSize),
                "decompress" => WorkerBuilder.Decompressor(),
                _            => throw new Exception("Unknown operation")
            };

            var worker = workerBuilder.Parallel()
                                      .BuildFor(inputFile, outputFile);

            Console.WriteLine("Starting...");
            var sw = Stopwatch.StartNew();
            worker.Run();
            var elapsed = sw.Elapsed;

            Console.WriteLine($"Done in {elapsed.TotalSeconds:F3} sec");
        }
    }
}