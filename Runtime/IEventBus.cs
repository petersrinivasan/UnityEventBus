namespace Petersrin.EventBus
{
    public interface IEventBus
    {
        int DelegateLookupCount { get; }

        void Raise(Event e);
        void Subscribe<T>(EventBus.EventDelegate<T> del) where T : Event;
        void Unsubscribe<T>(EventBus.EventDelegate<T> del) where T : Event;
    }
}