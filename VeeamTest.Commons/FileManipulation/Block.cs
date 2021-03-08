namespace VeeamTest.Commons.FileManipulation
{
    /// <summary>Блок данных</summary>
    public class Block
    {
        public Block(long Offset, byte[] Data)
        {
            this.Offset = Offset;
            this.Data   = Data;
        }

        public long   Offset { get; }
        public byte[] Data   { get; }

        public override string ToString()
        {
            return $"{Offset:x8} -> {Data.Length:x8} bytes";
        }
    }
}