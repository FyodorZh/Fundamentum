using System;

namespace Actuarius.Concurrent
{
    /// <summary>
    /// Позволяет многим потокам, независимо друг от друга открывать и закрывать критические секции
    /// Параллельно, можно послать сигнал финализации структуры, который приведёт к однократному вызову пользовательского метода.
    /// Гарантируется, что пользовательский метод будет вызван вне критических секций.
    /// Если в момент попытки финализации структуры была активна одна или несколько критических секций, то финализация будет отложена на закрытие последней сессии.
    /// После отправки сигнала к финализации, новые секции открыть становится невозможно.
    /// </summary>
    public class ConcurrentManualLockGate
    {
        private readonly Action _onClose;

        private volatile int _state;

        public ConcurrentManualLockGate(Action onClose)
        {
            _onClose = onClose;
            _state = EncodeState(true, 0);
            Enter();
        }

        private static void DecodeState(int state, out bool isOpen, out int count)
        {
            isOpen = (state & 1) != 0;
            count = state >> 1;
        }

        private static int EncodeState(bool isOpen, int count)
        {
            return (count << 1) + (isOpen ? 1 : 0);
        }

        public void TryClose()
        {
            while (true)
            {
                int oldState = _state;
                DecodeState(oldState, out var isOpen, out var count);

                if (isOpen)
                {
                    isOpen = false;
                    if (System.Threading.Interlocked.CompareExchange(ref _state, EncodeState(isOpen, count), oldState) == oldState)
                    {
                        Exit();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public bool Enter()
        {
            while (true)
            {
                int oldState = _state;
                DecodeState(oldState, out var isOpen, out var count);

                if (isOpen)
                {
                    count += 1;
                    if (System.Threading.Interlocked.CompareExchange(ref _state, EncodeState(isOpen, count), oldState) == oldState)
                    {
                        return true;
                    }
                    continue;
                }

                break;
            }

            return false;
        }

        public void Exit()
        {
            while (true)
            {
                int oldState = _state;
                DecodeState(oldState, out var isOpen, out var count);

                count -= 1;

                if (System.Threading.Interlocked.CompareExchange(ref _state, EncodeState(isOpen, count), oldState) == oldState)
                {
                    if (count == 0)
                    {
                        _onClose();
                    }

                    break;
                }
            }
        }
    }
}