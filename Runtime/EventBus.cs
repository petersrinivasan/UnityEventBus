// THANKS to Will R. Miller for the original type-safe event manager for Unity
// THANKS to Robert Wahler for the original implementation of Miller's work.

using System.Collections.Generic;

namespace Petersrin.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<System.Type, EventDelegate> 
            _delegates = new Dictionary<System.Type, EventDelegate>();
    
        private readonly Dictionary<System.Delegate, EventDelegate>
            _subscribers = new Dictionary<System.Delegate, EventDelegate>();

        public delegate void EventDelegate<T>(T e) where T : Event;

        private delegate void EventDelegate(Event e);

        public int DelegateLookupCount => _subscribers.Count;

        public void Subscribe<T>(EventDelegate<T> del) where T : Event
        {
            if (_subscribers.ContainsKey(del)) return;

            EventDelegate internalDelegate = e => del((T) e);
            _subscribers[del] = internalDelegate;

            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                // ReSharper disable once RedundantAssignment
                _delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                _delegates[typeof(T)] = internalDelegate;
            }
        }

        public void Unsubscribe<T>(EventDelegate<T> del) where T : Event
        {

            EventDelegate internalDelegate;
            if (!_subscribers.TryGetValue(del, out internalDelegate)) return;
            
            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null) _delegates.Remove(typeof(T));
                else _delegates[typeof(T)] = tempDel;
            }

            _subscribers.Remove(del);
        }
        public void Raise(Event e)
        {
            if (_delegates.TryGetValue(e.GetType(), out var del)) del.Invoke(e);
        }
    }
}