using VeeamTest.Commons.FileManipulation;

namespace VeeamTest.Commons.Processing
{
    public interface IProcessor
    {
        byte[] Process(byte[] Input, int Offset, int Count);
    }

    public static class ProcessorExtensions
    {
        public static byte[] Process(this IProcessor Processor, byte[] Input)
        {
            return Processor.Process(Input, 0, Input.Length);
        }

        public static Block Process(this IProcessor Processor, Block Block)
        {
            return new(Block.Offset, Processor.Process(Block.Data));
        }
    }
}