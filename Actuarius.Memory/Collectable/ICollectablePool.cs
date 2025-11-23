namespace Actuarius.Memory
{
    public interface ICollectablePool
    {
        TResource Acquire<TResource>() where TResource : class, ICollectableResource<TResource>, new();
    }
}