using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhikozzLib
{
    public class EventManager : MonoBehaviour, IEventService, IServiceRegister
    {
        private class EventUnsubscriber : MonoBehaviour
        {
            private readonly List<Action> _unsubscribers = new();

            public void Track(Action unsubscribe)
            {
                _unsubscribers.Add(unsubscribe);
            }

            private void OnDestroy()
            {
                foreach (var unsubscribe in _unsubscribers)
                {
                    unsubscribe();
                }
            }
        }

        private readonly Dictionary<Type, Delegate> _handlers = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IEventService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.Unregister<IEventService>();
        }

        public void Subscribe<T>(Component owner, Action<T> handler) where T : struct
        {
            Subscribe(handler);

            if (!owner.TryGetComponent<EventUnsubscriber>(out var unsubscriber))
            {
                unsubscriber = owner.gameObject.AddComponent<EventUnsubscriber>();
            }

            unsubscriber.Track(() => Unsubscribe(handler));
        }

        public void Subscribe<T>(Action<T> handler) where T : struct
        {
            Type eventType = typeof(T);

            if (_handlers.TryGetValue(eventType, out Delegate existing))
            {
                _handlers[eventType] = Delegate.Combine(existing, handler);
            }
            else
            {
                _handlers[eventType] = handler;
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            Type eventType = typeof(T);

            if (!_handlers.TryGetValue(eventType, out Delegate existing))
            {
                return;
            }

            Delegate updated = Delegate.Remove(existing, handler);

            if (updated == null)
            {
                _handlers.Remove(eventType);
            }
            else
            {
                _handlers[eventType] = updated;
            }
        }

        public void Publish<T>(T evt)  where T : struct
        {
            Type eventType = typeof(T);

            if (_handlers.TryGetValue(eventType, out Delegate existing))
            {
                if (existing is Action<T> callback)
                {
                    callback.Invoke(evt);
                }
            }
        }

        public void Clear()
        {
            _handlers.Clear();
        }
    }
}