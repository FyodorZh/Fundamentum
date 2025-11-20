using Actuarius.Collections;

namespace Actuarius.Memory
{
    public interface IReadOnlyByteArray: IReadOnlyBytes, IReadOnlyArray<byte>
    {
        byte[] ReadOnlyArray { get; }
        int Offset { get; }
    }
}