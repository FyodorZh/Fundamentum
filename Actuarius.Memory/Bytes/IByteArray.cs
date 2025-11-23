using System;
using Actuarius.Collections;

namespace Actuarius.Memory
{
    public interface IByteArray : IReadOnlyByteArray, IArray<byte>
    {
        byte[] Array { get; }
    }

    public static class IByteArray_Ext
    {
        public static bool CopyFrom(this IByteArray dst, byte[] src, int srcOffset, int dstOffset, int count)
        {
            Buffer.BlockCopy(src, srcOffset, dst.Array, dstOffset, count);
            return true;
        }
    }
}