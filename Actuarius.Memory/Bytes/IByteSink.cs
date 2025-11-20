using Actuarius.Collections;

namespace Actuarius.Memory
{
    /// <summary>
    /// Аккумулятор байтов
    /// </summary>
    public interface IByteSink : IConsumer<byte>
    {
        /// <summary>
        /// Количество байт доступных для записи
        /// </summary>
        //int Capacity { get; }
        
        /// <summary>
        /// Вставляет массив байт.
        /// </summary>
        /// <param name="bytes"></param>
        bool PutMany<TBytes>(TBytes bytes) where TBytes : IReadOnlyBytes;
    }
}