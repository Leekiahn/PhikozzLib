using System;
using System.Collections.Generic;
using PhikozzLib;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventService, IServiceRegister
{
    private readonly Dictionary<Type, Delegate> _handlers = new Dictionary<Type, Delegate>();

    public void RegisterService()
    {
        ServiceLocator.Register<IEventService>(this);
    }

    public void Subscribe<T>(Action<T> handler)  where T : BaseEvent
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

    public void Unsubscribe<T>(Action<T> handler)  where T : BaseEvent
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

    public void Publish<T>(T evt)  where T : BaseEvent
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

    private void OnDestroy()
    {
        _handlers.Clear();
    }
}