using System;
using Actuarius.Collections;

namespace Actuarius.Memory
{
    public class DelegatePool<TResource> : Pool<TResource>
        where TResource : class
    {
        private readonly Func<TResource> _ctor;
        
        public DelegatePool(Func<TResource> resourceCtor, Func<TResource, bool>? deInitializer)
            : this(resourceCtor, new CycleQueue<TResource>(), deInitializer)
        {
        }

        public DelegatePool(Func<TResource> resourceCtor, IUnorderedCollection<TResource> pool, Func<TResource, bool>? deInitializer)
            :base(pool, deInitializer)
        {
            _ctor = resourceCtor;
        }

        protected override TResource Constructor()
        {
            return _ctor();
        }
    }
}