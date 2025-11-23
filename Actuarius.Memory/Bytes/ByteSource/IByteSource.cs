using Actuarius.Collections;

namespace Actuarius.Memory
{
    public interface IByteSource : IProducer<byte>
    {
        bool TakeMany(IMultiRefByteArray dst);
    }
}