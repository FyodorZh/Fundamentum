namespace Actuarius.Memory
{
    public interface IMultiRefResourceOwner<out TResource> : IMultiRefResource
        where TResource : class
    {
        TResource ShowResourceUnsafe();
    }

    public static class IMultiRefResourceOwner_Ext
    {
        public static ReleasableResourceAccessor<TResource> GetAccessor<TResource>(this IMultiRefResourceOwner<TResource> owner)
            where TResource : class
        {
            return new ReleasableResourceAccessor<TResource>(owner.ShowResourceUnsafe(), owner.Acquire());
        }
    }
}