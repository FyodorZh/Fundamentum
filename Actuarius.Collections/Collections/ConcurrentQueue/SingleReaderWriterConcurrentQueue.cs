using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Actuarius.Collections
{
    /// <summary>
    /// Очередь с одним продюсером и одним консюмером.
    /// В случае конфликтов приоритет получает продюсер
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class SingleReaderWriterConcurrentQueue<TData> : ISingleReaderWriterConcurrentQueue<TData>
    {
        private volatile CycleQueue<TData> _writeDst;
        private volatile CycleQueue<TData> _readSrc;
        private volatile CycleQueue<TData> _writeDstRef;

        private volatile bool mReadyToSwap;

        public int Count
        {
            get
            {
                CycleQueue<TData> writer = _writeDst;
                CycleQueue<TData> reader = _readSrc;
                while (ReferenceEquals(writer, reader))
                {
                    writer = _writeDst;
                    reader = _readSrc;
                }

                return writer.Count + reader.Count; // maybe inaccurate
            }
        }

        public SingleReaderWriterConcurrentQueue(int initialCapacity = 10)
        {
            _writeDst = new CycleQueue<TData>(initialCapacity);
            _readSrc = new CycleQueue<TData>(initialCapacity);

            _writeDstRef = _writeDst;
        }

        public bool Put(TData value)
        {
            var placeToWrite = Interlocked.Exchange(ref _writeDst!, null);
            var res = placeToWrite!.Put(value);
            mReadyToSwap = mReadyToSwap || res;
            Interlocked.Exchange(ref _writeDst, placeToWrite);
            return res;
        }

        public bool TryPop([MaybeNullWhen(false)] out TData value)
        {
            if (_readSrc.TryPop(out value))
            {
                return true;
            }

            if (mReadyToSwap)
            {
                mReadyToSwap = false;
                var oldReadSrc = _readSrc;
                while (Interlocked.CompareExchange(ref _writeDst, _readSrc, _writeDstRef) != _writeDstRef)
                {
                    //System.Threading.Thread.Yield();
                }

                _readSrc = _writeDstRef;
                _writeDstRef = oldReadSrc;

                return _readSrc.TryPop(out value);
            }

            return false;
        }
    }
}