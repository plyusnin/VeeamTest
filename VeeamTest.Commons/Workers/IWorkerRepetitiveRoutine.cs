namespace VeeamTest.Commons.Workers
{
    public interface IWorkerRepetitiveRoutine
    {
        IterationResult Iterate();
    }

    public enum IterationResult
    {
        Continue,
        Break
    }
}