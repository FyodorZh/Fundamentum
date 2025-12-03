using System;
using Actuarius.Collections;

namespace Actuarius.Memory.ConcurrentBuffered
{
    public class BucketSource<TObject>
    {
        private readonly int _bucketSize;
        private readonly Func<TObject> _objectCtor;

        private readonly IConcurrentUnorderedCollection<Bucket<TObject>> _fullBuckets;
        private readonly IConcurrentUnorderedCollection<Bucket<TObject>> _emptyBuckets;

        public BucketSource(int bucketSize, Func<TObject> objectCtor, Func<IConcurrentUnorderedCollection<Bucket<TObject>>> ctor)
        {
            _bucketSize = bucketSize;
            _objectCtor = objectCtor;
            _fullBuckets = ctor.Invoke();
            _emptyBuckets = ctor.Invoke();
        }

        public Bucket<TObject> GetFullBucket()
        {
            if (!_fullBuckets.TryPop(out var bucket))
            {
                bucket = GetEmptyBucket();
                bucket.LazyFill();
            }

            return bucket;
        }

        public Bucket<TObject> GetEmptyBucket()
        {
            if (!_emptyBuckets.TryPop(out var bucket))
            {
                bucket = new Bucket<TObject>(_bucketSize, _objectCtor);
            }

            return bucket;
        }

        public bool ReturnFullBucket(Bucket<TObject> bucket)
        {
            return _fullBuckets.Put(bucket);
        }

        public bool ReturnEmptyBucket(Bucket<TObject> bucket)
        {
            return _emptyBuckets.Put(bucket);
        }
    }
}