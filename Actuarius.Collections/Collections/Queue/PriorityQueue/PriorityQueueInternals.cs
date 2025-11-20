using System;
using System.Collections.Generic;

namespace Actuarius.Collections
{
    public partial class PriorityQueue<Key, Data> : PriorityQueue<Key, Data>.IPriorityQueueCtl
        where Key : IComparable<Key>
    {
        protected interface IPriorityQueueCtl
        {
            bool RemoveAtIndex(int id);
            bool UpdateKeyAtIndex(int id, Key key);
            Key KeyAt(int idx);
            Data DataAt(int idx);
        }

        private PriorityQueue(PriorityQueue<Key, Data> queue)
        {
            _keys.AddRange(queue._keys);
            _data.AddRange(queue._data);
        }

        private Key[] _Keys
        {
            get
            {
                PriorityQueue<Key, Data> queue = new PriorityQueue<Key, Data>(this);
                List<Key> keys = new List<Key>();
                while (queue.Count > 0)
                {
                    keys.Add(queue.TopKey());
                    queue.Dequeue();
                }
                return keys.ToArray();
            }
        }

        public struct DataRef
        {
            private IPriorityQueueCtl? _owner;
            private readonly int _id;

            public DataRef(PriorityQueue<Key, Data> owner, int id)
            {
                _owner = owner;
                _id = id;
            }

            public bool IsValid => _owner != null;

            public Data ShowData()
            {
                if (_owner != null)
                {
                    return _owner.DataAt(_id);
                }
                throw new NullReferenceException();
            }

            public Key ShowKey()
            {
                if (_owner != null)
                {
                    return _owner.KeyAt(_id);
                }
                throw new NullReferenceException();
            }

            public bool Update(Key newKey)
            {
                if (_owner != null)
                {
                    var res = _owner.UpdateKeyAtIndex(_id, newKey);
                    _owner = null;
                    return res;
                }
                return false;
            }

            public bool Remove()
            {
                if (_owner != null)
                {
                    return _owner.RemoveAtIndex(_id);
                }
                return false;
            }
        }

        public struct Enumerator : IEnumerator<DataRef>
        {
            private readonly PriorityQueue<Key, Data> _owner;

            private readonly int _lastExclusiveId;
            private readonly int _stride;

            private int _index;
            private DataRef _current;

            public Enumerator(PriorityQueue<Key, Data> owner, int beforeFirstId, int lastExclusiveId)
            {
                _owner = owner;

                _lastExclusiveId = lastExclusiveId;
                _stride = (beforeFirstId < lastExclusiveId) ? 1 : -1;

                _index = beforeFirstId;
                _current = default(DataRef);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                _index += _stride;
                if (_index != _lastExclusiveId)
                {
                    _current = new DataRef(_owner, _index);
                    return true;
                }

                _index = _lastExclusiveId - _stride;
                _current = default;
                return false;
            }

            public DataRef Current => _current;

            object System.Collections.IEnumerator.Current => throw new InvalidOperationException("Don't use this getter");

            void System.Collections.IEnumerator.Reset()
            {
                _index = 0;
                _current = default;
            }
        }

        public readonly struct Enumerable
        {
            private readonly PriorityQueue<Key, Data> _owner;
            private readonly QueueEnumerationOrder _order;

            public Enumerable(PriorityQueue<Key, Data> owner, QueueEnumerationOrder order)
            {
                _owner = owner;
                _order = order;
            }

            public Enumerator GetEnumerator()
            {
                switch (_order)
                {
                    case QueueEnumerationOrder.HeadToTail:
                        return new Enumerator(_owner, 0, _owner.Count + 1);
                    case QueueEnumerationOrder.TailToHead:
                        return new Enumerator(_owner, _owner.Count + 1, 0);
                    default:
                        throw new InvalidOperationException(_order.ToString());
                }
            }
        }

        public readonly struct DataEnumerable
        {
            private readonly List<Data> _data;
            public DataEnumerable(List<Data> data)
            {
                _data = data;
            }

            public List<Data>.Enumerator GetEnumerator()
            {
                var en = _data.GetEnumerator();
                en.MoveNext();
                return en;
            }
        }

//         [UT.UT("PriorityQueue")]
//         private static void UT(UT.IUTest test)
//         {
//             PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
//             test.Equal(queue.Count, 0);
//
//             System.Collections.Generic.SortedList<int, int> list = new System.Collections.Generic.SortedList<int, int>();
//             int val;
//
//             for (int i = 1; i < 10000; ++i)
//             {
//                 val = i;
//
//                 queue.Enqueue(val, val);
//                 list.Add(val, val);
//
//                 val = -i;
//
//                 queue.Enqueue(val, val);
//                 list.Add(val, val);
//
//                 int a = queue.Dequeue();
//                 var en = list.GetEnumerator();
//                 en.MoveNext();
//                 int b = en.Current.Value;
//                 list.Remove(b);
//
//                 test.Equal(a, b);
//             }
//
//             /*
//             System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
//             sw.Reset();
//             sw.Start();
//             for (int i = 1; i < 10000; ++i)
//             {
//                 val = i;
//                 queue.Enqueu(val, val);
//                 val = -i;
//                 queue.Enqueu(val, val);
//                 val = queue.Dequeue();
//             }
//             sw.Stop();
//             Log.i("Time " + sw.ElapsedMilliseconds);
//                         
//             System.Collections.Generic.SortedDictionary<int, int> sdic = new System.Collections.Generic.SortedDictionary<int, int>();
//
//             sw.Reset();
//             sw.Start();
//             for (int i = 1; i < 10000; ++i)
//             {
//                 val = i;
//                 sdic.Add(val, val);
//                 val = -i;
//                 sdic.Add(val, val);
//                 var en = sdic.GetEnumerator();
//                 en.MoveNext();
//                 val = en.Current.Value;
//                 sdic.Remove(val);
//             }
//             sw.Stop();
//             Log.i("Time " + sw.ElapsedMilliseconds);
//
//             System.Collections.Generic.SortedList<int, int> slist = new System.Collections.Generic.SortedList<int, int>();
//
//             sw.Reset();
//             sw.Start();
//             for (int i = 1; i < 10000; ++i)
//             {
//                 val = i;
//                 slist.Add(val, val);
//                 val = -i;
//                 slist.Add(val, val);
//                 var en = slist.GetEnumerator();
//                 val = en.Current.Value;
//                 en.MoveNext();
//                 slist.Remove(val);
//             }
//             sw.Stop();
//             Log.i("Time " + sw.ElapsedMilliseconds);
//             */
//         }
    }
}