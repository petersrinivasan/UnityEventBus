namespace Petersrin.EventBus
{
    public interface IEventBusSubscriber
    {
        void SubscribeEvents();
        void UnsubscribeEvents();
    }
}