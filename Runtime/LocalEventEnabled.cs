using UnityEngine;

namespace Petersrin.EventBus
{
    public abstract class LocalEventEnabled : MonoBehaviour
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected IEventBus EventBus;

        protected virtual void OnEnable() => EventBus = GetComponentInParent<IEventBus>();
    }
}