namespace VeeamTest.Commons.Workers
{
    public abstract class WorkerBase<TRoutine> : IWorker
        where TRoutine : IWorkerRepetitiveRoutine
    {
        private readonly TRoutine _routine;

        protected WorkerBase(TRoutine Routine)
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