namespace Fundamentum.Collections
{
    public interface IReadOnlyArray<out TData> : ICountable
    {
        TData this[int id] { get; }
    }
}