namespace Petersrin.EventBus
{
    public class GlobalEventBus : Singleton<GlobalEventBus>, IEventBus
    {
        private readonly IEventBus _eventBus = new EventBus();
        
        public int DelegateLookupCount => _eventBus.DelegateLookupCount;

        public void Subscribe<T>(EventBus.EventDelegate<T> del) where T : Event => _eventBus.Subscribe(del);

        public void Unsubscribe<T>(EventBus.EventDelegate<T> del) where T : Event => _eventBus.Unsubscribe(del);

        public void Raise(Event e) => _eventBus.Raise(e);
    }
}