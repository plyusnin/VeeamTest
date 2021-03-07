using System.Threading;

namespace VeeamTest.Commons.FileManipulation.Lockable
{
    /// <summary>Делает указанный <see cref="IBlockSource" /> потокобезопасным</summary>
    /// <remarks>
    ///     Использует <see cref="SemaphoreSlim" /> для обеспечения потокобезопасности, блокирует запрашиваемый поток до
    ///     тех пор, пока другой поток не закончит выполнение операции.
    /// </remarks>
    public class LockedBlockSourceDecorator : IBlockSource
    {
        private readonly IBlockSource _core;
        private readonly SemaphoreSlim _semaphore;

        public LockedBlockSourceDecorator(IBlockSource Core)
        {
            _core      = Core;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public Block? Take()
        {
            _semaphore.Wait();
            try
            {
                return _core.Take();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public override string ToString()
        {
            return $"[Locked {_core}]";
        }
    }
}