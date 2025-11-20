using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Actuarius.Collections
{
    public class SystemStack<TData> : IStack<TData>
    {
        private readonly Stack<TData> _stack = new Stack<TData>();
        
        public int Count => _stack.Count;

        public bool Put(TData value)
        {
            _stack.Push(value);
            return true;
        }

        public bool TryPop([MaybeNullWhen(false)] out TData value)
        {
            if (_stack.Count > 0)
            {
                value = _stack.Pop();
                return true;
            }

            value = default;
            return false;
        }
    }
}