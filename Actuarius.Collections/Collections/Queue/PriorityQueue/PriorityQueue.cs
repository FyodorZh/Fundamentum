using System;
using System.Collections.Generic;

namespace Actuarius.Collections
{
    /// <summary>
    /// Приоритетная очередь. Чем меньше ключ, тем он приоритетнее
    /// </summary>
    public partial class PriorityQueue<Key, Data> : IQueue<KeyValuePair<Key, Data>>
        where Key : IComparable<Key>
    {
        private readonly List<Key> _keys = new List<Key>();
        private readonly List<Data> _data = new List<Data>();

        public PriorityQueue()
        {
            Clear();
        }
        
        public void Clear()
        {
            _keys.Clear();
            _data.Clear();
            _keys.Add(default!);
            _data.Add(default!);
        }

        public int Count => _keys.Count - 1;

        public void Enqueue(Key key, Data data)
        {
            _keys.Add(key);
            _data.Add(data);
            Up(_keys.Count - 1);
        }

        public Key TopKey()
        {
            if (Count != 0)
            {
                return _keys[1];
            }
            throw new IndexOutOfRangeException();
        }

        public Data Peek()
        {
            if (Count != 0)
            {
                return _data[1];
            }
            throw new IndexOutOfRangeException();
        }

        public Data Dequeue()
        {
            TryPop(out var res);
            return res.Value;
        }

        public bool Put(KeyValuePair<Key, Data> value)
        {
            Enqueue(value.Key, value.Value);
            return true;
        }

        public bool TryPop(out KeyValuePair<Key, Data> value)
        {
            int count = Count;
            if (count != 0)
            {
                value = new KeyValuePair<Key, Data>(_keys[1], _data[1]);
                _keys[1] = _keys[count];
                _data[1] = _data[count];
                _keys.RemoveAt(count);
                _data.RemoveAt(count);
                Down(1);
                return true;
            }
            value = default(KeyValuePair<Key, Data>);
            return false;
        }

        public Enumerable Enumerate(QueueEnumerationOrder order)
        {
            return new Enumerable(this, order);
        }

        public DataEnumerable Values => new(_data);

        Key IPriorityQueueCtl.KeyAt(int idx)
        {
            return _keys[idx];
        }

        Data IPriorityQueueCtl.DataAt(int idx)
        {
            return _data[idx];
        }

        bool IPriorityQueueCtl.RemoveAtIndex(int id)
        {
            if (id < 1 || id >= _keys.Count)
            {
                return false;
            }

            Swap(id, _keys.Count - 1);
            _keys.RemoveAt(_keys.Count - 1);
            _data.RemoveAt(_data.Count - 1);

            Down(id);
            return true;
        }

        bool IPriorityQueueCtl.UpdateKeyAtIndex(int id, Key key)
        {
            if (id < 1 || id >= _keys.Count)
            {
                return false;
            }

            int cmp = _keys[id].CompareTo(key);
            _keys[id] = key;
            if (cmp < 0)
            {
                Down(id);
            }
            else if (cmp > 0)
            {
                Up(id);
            }
            return true;
        }

        private void Up(int id)
        {
            while (id > 1)
            {
                int top = id / 2;
                if (_keys[id].CompareTo(_keys[top]) < 0)
                {
                    Swap(id, top);
                    id = top;
                }
                else
                {
                    break;
                }
            }
        }

        private void Down(int id)
        {
            int count = Count;
            while (id <= count)
            {
                int a = id * 2;
                int minAB = id;
                if (a <= count)
                {
                    int b = a + 1;
                    if (b <= count)
                    {
                        minAB = _keys[a].CompareTo(_keys[b]) < 0 ? a : b;
                    }
                    else
                    {
                        minAB = a;
                    }
                }
                if (minAB != id && _keys[minAB].CompareTo(_keys[id]) < 0)
                {
                    Swap(minAB, id);
                    id = minAB;
                }
                else
                {
                    break;
                }
            }
        }

        private void Swap(int a, int b)
        {
            Key k = _keys[a];
            Data d = _data[a];
            _keys[a] = _keys[b];
            _data[a] = _data[b];
            _keys[b] = k;
            _data[b] = d;
        }
    }
}
