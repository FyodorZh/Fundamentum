using System;
using Actuarius.Collections;

namespace Actuarius.Memory
{
    public class RawArrayPool<T> : Pool<T[], int>
    {
        private readonly Func<T[], bool>? _deInitializer;

        public RawArrayPool(Func<T[], bool>? deInitializer)
            : base(new SystemDictionary<int, IPool<T[]>>())
        {
            _deInitializer = deInitializer;
        }
        
        protected override IPool<T[]> CreatePool(int classId)
        {
            return new FixedLengthRawArrayPool<T>(classId, _deInitializer);
        }

        protected sealed override int Classify(int param0)
        {
            return BitMath.NextPow2((uint)param0);
        }

        protected sealed override int Classify(T[] array)
        {
            return array.Length;
        }
    }
}