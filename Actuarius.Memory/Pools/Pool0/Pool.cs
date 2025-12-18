using System;
using Actuarius.Collections;

namespace Actuarius.Memory
{
    public abstract class Pool<TResource> : IPool<TResource>
        where TResource : class
    {
        private readonly IUnorderedCollection<TResource> _pool;
        private readonly Func<TResource, bool>? _deInitializer;
        
        protected abstract TResource Constructor();
        
        protected Pool(Func<TResource, bool>? deInitializer)
            : this(new CycleQueue<TResource>(), deInitializer)
        {
        }

        protected Pool(IUnorderedCollection<TResource> pool, Func<TResource, bool>? deInitializer)
        {
            _pool = pool;
            _deInitializer = deInitializer;
        }

        public void Release(TResource? resource)
        {
            if (resource != null)
            {
                bool putToPool = _deInitializer?.Invoke(resource) ?? true;
                if (putToPool)
                {
                    _pool.Put(resource);
                }
            }
        }

        public TResource Acquire()
        {
            if (_pool.TryPop(out var resource))
            {
                return resource;
            }
            return Constructor();
        }
    }
}