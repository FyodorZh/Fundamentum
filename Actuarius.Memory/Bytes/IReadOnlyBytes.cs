using Actuarius.Collections;

namespace Actuarius.Memory
{
    /// <summary>
    /// Абстрактное тредобезопасное хранилище последовательности байт.
    /// </summary>
    public interface IReadOnlyBytes : ICountable
    {
        /// <summary>
        /// False эквивалентно нулевому массиву
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Копирует данные в приёмник.
        /// </summary>
        /// <param name="dst"> Куда скопировать </param>
        /// <param name="dstOffset"> Начиная с какой позиции </param>
        /// <param name="srcOffset"> Номер первого копируемого элемента в источнике</param>
        /// <param name="count"> Количество байт для копирования</param>
        /// <returns> В случае неуспеха возвращает false. Данные в приёмнике остаются в неопределённом состоянии</returns>
        bool CopyTo(byte[] dst, int dstOffset, int srcOffset, int count);
    }
}