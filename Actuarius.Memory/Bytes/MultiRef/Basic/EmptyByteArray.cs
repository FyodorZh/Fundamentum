namespace Actuarius.Memory
{
    /// <summary>
    /// Эквивалентно new byte[0]
    /// Является синглтоном. Позволяет возвращать пустые массивы без аллокаций
    /// </summary>
    public class EmptyByteArray : MultiRefByteArray
    {
        public static readonly EmptyByteArray Instance = new EmptyByteArray();

        private EmptyByteArray()
            : base(System.Array.Empty<byte>(), 0, 0)
        {
        }

        protected override void OnReleased()
        {
            // DO NOTHING
        }
    }
}