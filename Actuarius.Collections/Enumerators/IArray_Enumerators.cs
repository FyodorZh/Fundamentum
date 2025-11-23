using System;
using System.Collections;
using System.Collections.Generic;

namespace Actuarius.Collections
{
    public static class IReadOnlyArray_Enumerators
    {
        public static AsEnumerable<TData> Enumerate<TData>(this IReadOnlyArray<TData> list)
        {
            return new AsEnumerable<TData>(list);
        }

        public readonly struct AsEnumerable<TData> : IEnumerable<TData>
        {
            private readonly IReadOnlyArray<TData> _array;

            public AsEnumerable(IReadOnlyArray<TData> array)
            {
                _array = array;
            }

            public Enumerator<TData> GetEnumerator()
            {
                return new Enumerator<TData>(_array);
            }

            IEnumerator<TData> IEnumerable<TData>.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public struct Enumerator<TData> : IEnumerator<TData>
        {
            private readonly IReadOnlyArray<TData> _array;
            private readonly int _count;
            private int _idx;

            public Enumerator(IReadOnlyArray<TData> array)
            {
                _array = array;
                _count = array.Count;
                _idx = -1;
            }

            public bool MoveNext()
            {
                _idx += 1;
                return _idx < _count;
            }

            public void Reset()
            {
                _idx = -1;
            }

            object? IEnumerator.Current => Current;

            public TData Current
            {
                get
                {
                    if (_idx >= 0 && _idx < _count)
                    {
                        return _array[_idx];
                    }

                    throw new IndexOutOfRangeException();
                }
            }

            public void Dispose()
            {
                // TODO release managed resources here
            }
        }
    }
}