using System;
using System.Diagnostics;
using System.IO;
using VeeamTest.Commons;
using VeeamTest.Commons.FileManipulation.Lego;
using VeeamTest.Commons.FileManipulation.Plain;
using VeeamTest.Commons.Processing;

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

            var worker = operation switch
            {
                "compress" => new Worker(
                    new PlainBlockSource(inputFile, _blockSize),
                    new LegoBlockSink(outputFile),
                    new GzipPackProcessor()),

                "decompress" => new Worker(
                    new LegoBlockSource(inputFile),
                    new PlainBlockSink(outputFile),
                    new GzipUnpackProcessor()),

                _ => throw new Exception("Unknown operation")
            };

            Console.WriteLine("Starting...");
            var sw = Stopwatch.StartNew();
            worker.Run();
            var elapsed = sw.Elapsed;

            Console.WriteLine($"Done in {elapsed.TotalSeconds:F3} sec");
        }
    }
}