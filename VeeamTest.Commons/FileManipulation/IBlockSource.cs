using System.Collections.Generic;

namespace VeeamTest.Commons.FileManipulation
{
    public interface IBlockSource
    {
        public Block? Take();
    }

    public static class BlockSourceExtensions
    {
        public static IEnumerable<Block> TakeAll(this IBlockSource Reader)
        {
            while (true)
            {
                var block = Reader.Take();
                if (block != null)
                    yield return block;
                else
                    yield break;
            }
        }
    }
}