// THANKS to Will R. Miller for the original type-safe event manager for Unity
// THANKS to Robert Wahler for the original implementation of Miller's work.

using System.Linq;
using UnityEngine;

namespace Petersrin.EventBus
{
    public class LocalEventBus : MonoBehaviour, IEventBus
    {
        private const string MultipleBuses = "Multiple LocalEventBuses in hierarchy. Unexpected behaviour may occur.";
        
        private readonly EventBus _eventBus = new EventBus();

        private void OnValidate()
        {
            var buses = GetComponentsInChildren<LocalEventBus>();
            if(buses.Count() > 1) Debug.LogWarning(MultipleBuses);
        }

        public int DelegateLookupCount => _eventBus.DelegateLookupCount;
        public void Subscribe<T>(EventBus.EventDelegate<T> del) where T : Event => _eventBus.Subscribe(del);
        public void Unsubscribe<T>(EventBus.EventDelegate<T> del) where T : Event => _eventBus.Unsubscribe(del);
        public void Raise(Event e) => _eventBus.Raise(e);
    }
}