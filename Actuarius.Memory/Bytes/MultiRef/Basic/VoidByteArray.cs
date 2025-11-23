using System;

namespace Actuarius.Memory
{
    /// <summary>
    /// Эквивалентно default(byte[])
    /// Является синглтоном. Позволяет возвращать невалидные массивы без аллокаций
    /// </summary>
    public class VoidByteArray : MultiRefByteArray
    {
        public static readonly MultiRefByteArray Instance = new VoidByteArray();

        private VoidByteArray()
            : base(null!, 0, 0)
        {
        }
    }
}