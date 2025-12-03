using System;
using System.Diagnostics.CodeAnalysis;

namespace Actuarius.Memory.ConcurrentBuffered
{
    public class Bucket<TObject>
    {
        private readonly Func<TObject> _ctor;
        private readonly TObject?[] _table;
        private readonly int _capacity;

        private int _realObjectsNumber;
        private int _virtualObjectsNumber;

        public Bucket(int capacity, Func<TObject> ctor)
        {
            _ctor = ctor;
            _table = new TObject[capacity];
            _capacity = capacity;
            _realObjectsNumber = 0;
            _virtualObjectsNumber = 0;
        }

        public void LazyFill()
        {
            _virtualObjectsNumber = _capacity - _realObjectsNumber;
        }

        public bool TryPop([MaybeNullWhen(false)] out TObject value)
        {
            if (_realObjectsNumber > 0)
            {
                int id = --_realObjectsNumber;
                value = _table[id]!;
                _table[id] = default;
                return true;
            }

            if (_virtualObjectsNumber > 0)
            {
                _virtualObjectsNumber -= 1;
                value = _ctor.Invoke();
                return true;
            }

            value = default;
            return false;
        }

        public bool Put(TObject value)
        {
            if (_realObjectsNumber == _capacity)
            {
                return false;
            }

            _table[_realObjectsNumber++] = value;
            if (_realObjectsNumber + _virtualObjectsNumber > _capacity)
            {
                _virtualObjectsNumber -= 1;
            }

            return true;
        }
    }
}