namespace Actuarius.Memory
{
    public interface IGenericPool<in TParam>
    {
        IPool<TResource, TParam> ShowTypedPool<TResource>();
        (TResource resource, IPoolSink<TResource> poolSink) Acquire<TResource>(TParam param);
    }
    
    public interface IGenericConcurrentPool<in TParam> : IGenericPool<TParam>
    {
        new IConcurrentPool<TResource, TParam> ShowTypedPool<TResource>();
    }
}