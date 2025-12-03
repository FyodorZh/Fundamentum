using System;

namespace Actuarius.Memory.ConcurrentBuffered
{
    internal class Pool<TObject>
    {
        private readonly BucketSource<TObject> mSource;

        public readonly int ID;

        private Bucket<TObject> _part0;
        private Bucket<TObject> _part1;

        public Pool(int id, BucketSource<TObject> bucketSource)
        {
            mSource = bucketSource;

            ID = id;
            _part0 = mSource.GetFullBucket();
            _part1 = mSource.GetEmptyBucket();
        }

        public TObject Acquire(out bool failedToReturnEmptyBucket)
        {
            failedToReturnEmptyBucket = false;

            if (!_part1.TryPop(out var obj) && !_part0.TryPop(out obj))
            {
                failedToReturnEmptyBucket = !mSource.ReturnEmptyBucket(_part0);
                _part0 = mSource.GetFullBucket();

                if (!_part0.TryPop(out obj))
                {
                    throw new InvalidOperationException("Failed to construct object (1)");
                }
            }

            return obj;
        }

        public void Release(TObject obj, out bool failedToReturnFullBucket, out bool emptyBucketOverflow)
        {
            failedToReturnFullBucket = false;
            emptyBucketOverflow = false;

            if (!_part0.Put(obj) && !_part1.Put(obj))
            {
                // Нет места

                failedToReturnFullBucket = !mSource.ReturnFullBucket(_part1);
                _part1 = mSource.GetEmptyBucket();

                emptyBucketOverflow = !_part1.Put(obj);
            }
        }
    }
}