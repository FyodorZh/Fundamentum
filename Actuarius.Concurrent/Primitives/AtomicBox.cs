using System.Threading;

namespace Actuarius.Concurrent
{
    /// <summary>
    /// Хранит данные, позволяет вычитывать и записывать их атомарно
    /// </summary>
    /// <typeparam name="T"> Данные </typeparam>
    public class AtomicBox<T>
        where T : struct
    {
        private T _value;

        private readonly ReaderWriterLockSlim mLock = new ReaderWriterLockSlim();

        public T Value
        {
            get
            {
                mLock.EnterReadLock();
                var res = _value;
                mLock.ExitReadLock();
                return res;
            }
            set
            {
                mLock.EnterWriteLock();
                _value = value;
                mLock.ExitWriteLock();
            }
        }
    }
}