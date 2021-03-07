using System;
using System.Threading;
using NUnit.Framework;
using VeeamTest.Commons.Workers;

namespace VeeamTest.Tests
{
    public class WorkerExceptionsTest
    {
        [Test]
        public void ExceptionInMainThreadTest()
        {
            var worker = CreateWorkerThatThrowsIf((local, main) => local == main);
            Assert.Throws<ApplicationException>(worker.Run);
        }

        [Test]
        public void ExceptionInAnotherThreadTest()
        {
            var worker = CreateWorkerThatThrowsIf((local, main) => local != main);
            Assert.Throws<ApplicationException>(worker.Run);
        }

        [Test]
        public void ExceptionInSingleThreadTest()
        {
            var routine = new DelegateRepetitiveRoutine(() => throw new ApplicationException());
            var worker  = new SingleThreadWorker(routine);
            Assert.Throws<ApplicationException>(worker.Run);
        }

        private static MultiThreadWorker CreateWorkerThatThrowsIf(Func<int, int, bool> Condition)
        {
            var mainThreadId = Thread.CurrentThread.ManagedThreadId;

            var routine = new DelegateRepetitiveRoutine(() =>
            {
                if (Condition(Thread.CurrentThread.ManagedThreadId, mainThreadId))
                    throw new ApplicationException();

                return IterationResult.Continue;
            });

            var worker = new MultiThreadWorker(routine, 2);
            return worker;
        }

        private class DelegateRepetitiveRoutine : IWorkerRepetitiveRoutine
        {
            private readonly Func<IterationResult> _delegate;

            public DelegateRepetitiveRoutine(Func<IterationResult> Delegate)
            {
                _delegate = Delegate;
            }

            public IterationResult Iterate()
            {
                return _delegate();
            }
        }
    }
}