using Actuarius.Collections;

namespace Actuarius.Memory
{
    public interface IByteArray : IReadOnlyByteArray, IArray<byte>
    {
        byte[] Array { get; }
    }
}