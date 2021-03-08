namespace VeeamTest.Commons.Workers
{
    /// <summary>Обработчик репитативной операции</summary>
    public abstract class WorkerBase : IWorker
    {
        private readonly IWorkerRepetitiveRoutine _routine;

        protected WorkerBase(IWorkerRepetitiveRoutine Routine)
        {
            _routine = Routine;
        }

        public abstract void Run();

        protected virtual void Routine()
        {
            bool runNext;
            do
            {
                runNext = _routine.Iterate() == IterationResult.Continue
                          && OtherChecks()   == IterationResult.Continue;
            } while (runNext);
        }

        protected virtual IterationResult OtherChecks()
        {
            return IterationResult.Continue;
        }
    }
}