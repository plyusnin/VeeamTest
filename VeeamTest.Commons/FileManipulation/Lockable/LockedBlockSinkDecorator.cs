using System.Threading;

namespace VeeamTest.Commons.FileManipulation.Lockable
{
    /// <summary>Делает указанный <see cref="IBlockSink" /> потокобезопасным</summary>
    /// <remarks>
    ///     Использует <see cref="SemaphoreSlim" /> для обеспечения потокобезопасности, блокирует запрашиваемый поток до
    ///     тех пор, пока другой поток не закончит выполнение операции.
    /// </remarks>
    public class LockedBlockSinkDecorator : IBlockSink
    {
        private readonly IBlockSink _core;
        private readonly SemaphoreSlim _semaphore;

        public LockedBlockSinkDecorator(IBlockSink Core)
        {
            _core      = Core;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public void Put(Block Block)
        {
            _semaphore.Wait();
            try
            {
                _core.Put(Block);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}