namespace Fundamentum.Collections
{
    public interface IStream<TData> : IConsumer<TData>, IProducer<TData>
    {
    }
}