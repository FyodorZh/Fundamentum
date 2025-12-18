using System;

namespace Actuarius.Memory
{
    public class FixedLengthRawArrayPool<T> : Pool<T[]>
    {
        private readonly int _length;
        
        public FixedLengthRawArrayPool(int length, Func<T[], bool>? deInitializer)
            :base(deInitializer)
        {
            _length = length;
        }

        protected override T[] Constructor()
        {
            return new T[_length];
        }
    }
}