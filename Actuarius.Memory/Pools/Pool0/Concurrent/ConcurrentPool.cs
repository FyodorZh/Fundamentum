using System;
using Actuarius.Collections;

namespace Actuarius.Memory
{
    public abstract class ConcurrentPool<TResource> : Pool<TResource>, IConcurrentPool<TResource>
        where TResource : class
    {
        protected ConcurrentPool(IConcurrentUnorderedCollection<TResource> pool, Func<TResource, bool>? deInitializer)
            : base(pool, deInitializer)
        {
        }
    }
}