namespace VeeamTest.Commons.Workers
{
    /// <summary>Представляет репитативную операцию</summary>
    public interface IWorkerRepetitiveRoutine
    {
        /// <summary>Тело репитативной операции</summary>
        /// <returns>Определяет, следует ли далее повторять операцию</returns>
        IterationResult Iterate();
    }

    public enum IterationResult
    {
        /// <summary>Повторять операцию ещё раз</summary>
        Continue,

        /// <summary>Закончить повторение операции</summary>
        Break
    }
}