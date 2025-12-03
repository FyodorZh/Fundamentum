using System.Threading;

namespace Actuarius.Memory
{
    public class GenericConcurrentPool : IGenericConcurrentPool
    {
        private static int _nextTypeId;
        
        // ReSharper disable once UnusedTypeParameter
        private static class TypeMap<TResource>
        {
            // ReSharper disable once StaticMemberInGenericType
            public static readonly int TypeId = Interlocked.Increment(ref _nextTypeId); 
        }

        private readonly object?[] _pools;

        private readonly int _bucketSize;
        private readonly int _distributionLevel;
           
        public GenericConcurrentPool(int typeCapacity, int bucketSize, int distributionLevel)
        {
            _pools = new object?[typeCapacity];
            _bucketSize = bucketSize;
            _distributionLevel = distributionLevel;
        }
        
        public IConcurrentPool<TResource> Acquire<TResource>() where TResource : class, new()
        {
            int typeId = TypeMap<TResource>.TypeId;
            
            // ReSharper disable once InconsistentlySynchronizedField
            if (_pools[typeId] == null)
            {
                lock (_pools)
                {
                    _pools[typeId] ??= new BufferedPool<TResource>(_bucketSize, _distributionLevel, () => new TResource());
                }
            }
            
            // ReSharper disable once InconsistentlySynchronizedField
            return (IConcurrentPool<TResource>)_pools[typeId]!;
        }

        IPool<TResource> IGenericPool.Acquire<TResource>()
        {
            return Acquire<TResource>();
        }
    }
}