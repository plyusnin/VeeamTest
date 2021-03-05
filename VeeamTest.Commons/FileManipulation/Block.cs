namespace VeeamTest.Commons.FileManipulation
{
    public class Block
    {
        public Block(long Offset, byte[] Data)
        {
            this.Offset = Offset;
            this.Data   = Data;
        }

        public long   Offset { get; }
        public byte[] Data   { get; }
    }
}