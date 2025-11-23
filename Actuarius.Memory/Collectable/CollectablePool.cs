using System;

namespace Actuarius.Memory
{
    public class CollectablePool : ICollectablePool
    {
        private readonly IConcurrentPool<object, Type> _pool;

        public CollectablePool(IConcurrentPool<object, Type> pool)
        {
            _pool = pool;
        }
        
        public TResource Acquire<TResource>() 
            where TResource : class, ICollectableResource<TResource>, new()
        {
            var obj = (TResource)_pool.Acquire(typeof(TResource));
            obj.Restored(_pool);
            return obj;
        }
    }
}