using System.Threading;

namespace Actuarius.Memory
{
    public interface IGenericPool
    {
        IPool<TResource> Acquire<TResource>() where TResource : class, new();
    }
    
    public interface IGenericConcurrentPool : IGenericPool
    {
        new IConcurrentPool<TResource> Acquire<TResource>() where TResource : class, new();
    }
}