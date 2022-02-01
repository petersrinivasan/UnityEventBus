namespace Petersrin.EventBus
{
    public abstract class LocalEventSubscriber : LocalEventEnabled, IEventBusSubscriber
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }

        protected virtual void OnDisable() => UnsubscribeEvents();

        public abstract void SubscribeEvents();
        public abstract void UnsubscribeEvents();
    }
}