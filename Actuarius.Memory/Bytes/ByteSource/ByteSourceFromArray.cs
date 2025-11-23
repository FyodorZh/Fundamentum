using System;

namespace Actuarius.Memory
{
    public class ByteSourceFromArray : MultiRefCollectableResource<ByteSourceFromArray>, IByteSource
    {
        private int _position;
        private IReadOnlyByteArray _array = null!;
        
        public void Reset(IReadOnlyByteArray array, int startPosition = 0)
        {
            _position = startPosition;
            _array = array;
        }

        public bool TryPop(out byte value)
        {
            if (_position < _array.Count)
            {
                value = _array[_position++];
                return true;
            }

            value = 0;
            return false;
        }

        public bool TakeMany(IMultiRefByteArray dst)
        {
            if (_position + dst.Count <= _array.Count)
            {
                Buffer.BlockCopy(_array.ReadOnlyArray, _array.Offset + _position, dst.Array, dst.Offset, dst.Count);
                _position += dst.Count;
                return true;
            }

            return false;
        }

        protected override void OnCollected()
        {
            _array = null!;
            _position = 0;
        }

        protected override void OnRestored()
        {
            // DO NOTHING
        }
    }
}