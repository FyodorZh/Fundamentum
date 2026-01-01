namespace Actuarius.Memory
{
    public struct ByteSinkToArray : IByteSink
    {
        private int _position;
        private readonly byte[] _array;
        private readonly int _count;
        private readonly int _offset;
        
        public ByteSinkToArray(byte[] array, int offset = 0, int count = -1)
        {
            _array = array;
            _offset = offset;
            _count = count < 0 ? array.Length : count;
            _position = 0;
        }

        public bool Put(byte value)
        {
            if (_position + _offset < _count)
            {
                _array[_position++] = value;
                return true;
            }

            return false;
        }

        public bool PutMany<TBytes>(TBytes bytes) where TBytes : IReadOnlyBytes
        {
            if (_position + _offset + bytes.Count <= _count)
            {
                if (bytes.CopyTo(_array, _position + _offset, 0, bytes.Count))
                {
                    _position += bytes.Count;
                    return true;
                }
            }

            return false;
        }
    }
}