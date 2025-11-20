namespace Actuarius.Collections
{
    public interface ISet<in TData> : IConsumer<TData>
    {
        bool Remove(TData element);
        bool Contains(TData element);
    }

    public interface IConcurrentSet<in TData> : ISet<TData>, IConcurrentConsumer<TData>
    {
    }
}