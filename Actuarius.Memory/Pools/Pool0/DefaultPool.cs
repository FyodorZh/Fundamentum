namespace Actuarius.Memory
{
    public class DefaultPool<TResource> : DelegatePool<TResource>
        where TResource : class, new()
    {
        public DefaultPool()
            : base(() => new())
        {
        }
    }
}