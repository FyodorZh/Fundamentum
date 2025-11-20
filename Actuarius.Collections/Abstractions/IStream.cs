namespace Actuarius.Collections
{
    public interface IStream<TData> : IConsumer<TData>, IProducer<TData>
    {
    }

    public interface ISingleReaderWriterConcurrentStream<TData> : IStream<TData>
    {
    }

    public interface IConcurrentStream<TData> : ISingleReaderWriterConcurrentStream<TData>, IConcurrentConsumer<TData>, IConcurrentProducer<TData>
    {
    }
}