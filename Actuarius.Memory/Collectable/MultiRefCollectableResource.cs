namespace Actuarius.Memory
{
    public abstract class MultiRefCollectableResource<TSelf> : MultiRefResource, ICollectableResource<TSelf>
        where TSelf : MultiRefCollectableResource<TSelf>
    {
        private IPoolSink<TSelf>? _owner;

        protected MultiRefCollectableResource()
            : base(true)
        {
        }

        protected abstract void OnCollected();
        protected abstract void OnRestored();

        protected sealed override void OnReleased()
        {
            _owner?.Release((TSelf)this); // Чтобы убрать этот каст надо использовать ко и контрвариантность
        }

        bool ICollectableResource<TSelf>.Collected()
        {
            OnCollected();
            _owner = null;
            return true;
        }

        void ICollectableResource<TSelf>.Restored(IPoolSink<TSelf> pool)
        {
            _owner = pool;
            Revive();
            OnRestored();
        }
    }
}