namespace Actuarius.Collections
{
    /// <summary>
    /// Очередь.
    /// Элементы изымаются в порядке добавления.
    /// Тредобезопасность не гарантируется
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IQueue<TData> : IUnorderedCollection<TData>, ICountable
    {
    }
    
    /// <summary>
    /// Тредобезопасная очередь, допускающая конкурентную запись и чтение,
    /// но при условии, что запись производится из одного потока, а чтение из другого 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface ISingleReaderWriterConcurrentQueue<TData> : IQueue<TData>, ISingleReaderWriterConcurrentUnorderedCollection<TData>
    {
    }

    /// <summary>
    /// Полностью тредобезопасная очередь
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IConcurrentQueue<TData> : ISingleReaderWriterConcurrentQueue<TData>, IConcurrentUnorderedCollection<TData>
    {
    }
}