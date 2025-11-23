using System;
using System.Threading;

namespace Actuarius.Memory
{
    /// <summary>
    /// Любой объект допускающий владение собой
    /// </summary>
    public interface IReleasableResource
    {
        /// <summary>
        /// Информирует объект об завершении факта владения
        /// </summary>
        void Release();
    }

    public static class IReleasableResource_Ext
    {
        public static ResourceOwner<TResource> AsDisposable<TResource>(this TResource resource)
            where TResource : class, IReleasableResource
        {
            return new ResourceOwner<TResource>(resource);
        }
        

        public struct ResourceOwner<TResource> : IDisposable
            where TResource : class, IReleasableResource
        {
            private TResource? _resource;

            public readonly TResource Value => _resource ?? throw new NullReferenceException();
            
            public ResourceOwner(TResource resource)
            {
                _resource = resource;
            }

            public void Dispose()
            {
                var resource = Interlocked.Exchange(ref _resource, null);
                resource?.Release();
            }
        }
    }
}