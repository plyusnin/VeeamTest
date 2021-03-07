using System.Threading;

namespace VeeamTest.Commons.FileManipulation.Lockable
{
    public static class LockedManipulationExtensions
    {
        /// <summary>Делает указанный <see cref="IBlockSink" /> потокобезопасным</summary>
        /// <remarks>
        ///     Использует <see cref="SemaphoreSlim" /> для обеспечения потокобезопасности, блокирует запрашиваемый поток до
        ///     тех пор, пока другой поток не закончит выполнение операции.
        /// </remarks>
        public static IBlockSink Locked(this IBlockSink Core)
        {
            return new LockedBlockSinkDecorator(Core);
        }

        /// <summary>Делает указанный <see cref="IBlockSource" /> потокобезопасным</summary>
        /// <remarks>
        ///     Использует <see cref="SemaphoreSlim" /> для обеспечения потокобезопасности, блокирует запрашиваемый поток до
        ///     тех пор, пока другой поток не закончит выполнение операции.
        /// </remarks>
        public static IBlockSource Locked(this IBlockSource Core)
        {
            return new LockedBlockSourceDecorator(Core);
        }
    }
}