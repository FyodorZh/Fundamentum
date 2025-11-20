namespace Actuarius.Collections
{
    /// <summary>
    /// Элементы изымаются в порядке LIFO
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IStack<TData> : IUnorderedCollection<TData>, ICountable
    {
    }
    
    public interface ISingleReaderWriterConcurrentStack<TData> : IStack<TData>, ISingleReaderWriterConcurrentUnorderedCollection<TData>
    {
    }
    
    public interface IConcurrentStack<TData> : ISingleReaderWriterConcurrentStack<TData>,  IConcurrentUnorderedCollection<TData>
    {
    }
}