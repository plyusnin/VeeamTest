namespace VeeamTest.Commons.Workers
{
    public class SingleThreadWorker : WorkerBase<IWorkerRepetitiveRoutine>
    {
        public SingleThreadWorker(IWorkerRepetitiveRoutine Routine) : base(Routine) { }

        public override void Run()
        {
            Routine();
        }
    }
}