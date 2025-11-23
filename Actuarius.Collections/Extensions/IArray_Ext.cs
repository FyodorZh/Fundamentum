using System;

namespace Actuarius.Collections
{
    public static class IArray_Ext
    {
        public static bool EqualByContent<T>(this IReadOnlyArray<T>? self, IReadOnlyArray<T>? other)
            where T : IEquatable<T>
        {
            if ((self == null) != (other == null))
            {
                return false;
            }

            if (self == null && other == null)
            {
                return true;
            }

            if (self!.Count != other!.Count)
            {
                return false;
            }

            for (int i = 0; i < self.Count; ++i)
            {
                if (!self[i].Equals(other[i]))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}