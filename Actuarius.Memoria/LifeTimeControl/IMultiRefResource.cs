namespace Actuarius.Memoria
{
    /// <summary>
    /// Объект у которого есть много владельцев
    /// </summary>
    public interface IMultiRef : IReleasableResource
    {
        /// <summary>
        /// TRUE если объект имеет хотя бы одного владельца
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Увеличивает число владельцев на 1
        /// </summary>
        void AddRef();
    }
}