namespace VeeamTest.Commons.Workers
{
    /// <summary>Выполняет репитативную операцию в текущем потоке</summary>
    public class SingleThreadWorker : WorkerBase
    {
        public SingleThreadWorker(IWorkerRepetitiveRoutine Routine) : base(Routine) { }

        public override void Run()
        {
            Routine();
        }
    }
}