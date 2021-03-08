using System;
using System.IO;
using System.Reflection;

namespace VeeamTest.Shell
{
    internal class Arguments
    {
        public static void PrintHelp()
        {
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            Console.WriteLine($"{exeName} OPERATION INPUT_FILE OUTPUT_FILE");
            Console.WriteLine("  OPERATION: Выполняемая операция");
            Console.WriteLine($"    {Operation.Compress} - сжатие файла");
            Console.WriteLine($"    {Operation.Decompress} - распаковка файла");
            Console.WriteLine("  INPUT_FILE: Входной файл");
            Console.WriteLine("  OUTPUT_FILE: Выходной файл");
        }

        public static (Operation Operation, string Input, string Output) ProcessArguments(string[] args)
        {
            if (args.Length < 3)
                throw new ApplicationException("Не указаны все параметры запуска");

            if (!Enum.TryParse<Operation>(args[0], true, out var operation))
                throw new ApplicationException($"Не поддерживаемый тип операции: {args[0]}");

            var inputFileName  = args[1];
            var outputFileName = args[2];

            if (!File.Exists(inputFileName))
                throw new ApplicationException($"Входной файл не существует: {inputFileName}");

            return (operation, inputFileName, outputFileName);
        }
    }
}