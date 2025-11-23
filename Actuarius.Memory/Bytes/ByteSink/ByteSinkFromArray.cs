namespace Actuarius.Memory
{
    public class ByteSinkFromArray : MultiRefCollectableResource<ByteSinkFromArray>, IByteSink
    {
        private int _position;
        private IByteArray _array = null!;
        
        public void Reset(IByteArray array, int startPosition = 0)
        {
            _position = startPosition;
            _array = array;
        }

        public bool Put(byte value)
        {
            if (_position < _array.Count)
            {
                _array[_position++] = value;
                return true;
            }

            return false;
        }

        public bool PutMany<TBytes>(TBytes bytes) where TBytes : IReadOnlyBytes
        {
            if (_position + bytes.Count <= _array.Count)
            {
                if (bytes.CopyTo(_array.Array, _array.Offset + _position, 0, bytes.Count))
                {
                    _position += bytes.Count;
                    return true;
                }
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