using System;
using Actuarius.Collections;

namespace Actuarius.Memory
{
    public class ConcurrentDelegatePool<TResource> : ConcurrentPool<TResource>
        where TResource : class
    {
        private readonly Func<TResource> _ctor;

        public ConcurrentDelegatePool(Func<TResource> resourceCtor, IConcurrentUnorderedCollection<TResource> pool, Func<TResource, bool>? deInitializer)
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