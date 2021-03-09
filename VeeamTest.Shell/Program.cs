using System;
using System.Diagnostics;
using System.IO;
using VeeamTest.Commons;

namespace VeeamTest.Shell
{
    internal class Program
    {
        private static readonly int _blockSize = 1024 * 1024;

        private static int Main(string[] args)
        {
            (Operation Operation, string Input, string Output) parameters;
            try
            {
                parameters = Arguments.ProcessArguments(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка в аргументах запуска программы: {e.Message}");
                Arguments.PrintHelp();
                return 1;
            }

            try
            {
                Console.WriteLine("Работаем...");

                var sw = Stopwatch.StartNew();
                Execute(parameters.Operation, parameters.Input, parameters.Output);
                sw.Stop();

                Console.WriteLine($"Закончили за {sw.Elapsed.TotalSeconds:F3} сек.");
                return 0;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"В ходе работы возникла ошибка: {e.Message}");
                Console.ResetColor();
                Console.Error.WriteLine(e.ToString());
                return 1;
            }
        }

        private static void Execute(Operation Operation, string InputFileName, string OutputFileName)
        {
            using var inputFile  = File.OpenRead(InputFileName);
            using var outputFile = File.Create(OutputFileName);

            var workerBuilder = Operation switch
            {
                Operation.Compress   => WorkerBuilder.Compressor(_blockSize),
                Operation.Decompress => WorkerBuilder.Decompressor(),
                _                    => throw new Exception("Неподдерживаемая операция")
            };

            var worker = workerBuilder.Parallel()
                                      .BuildFor(inputFile, outputFile);

            worker.Run();
        }
    }

    public enum Operation
    {
        Compress,
        Decompress
    }
}