using System;

public interface IEventService 
{
    void Subscribe<T>(Action<T> handler) where T : BaseEvent;
    void Unsubscribe<T>(Action<T> handler) where T : BaseEvent;
    void Publish<T>(T evt) where T : BaseEvent;
    void Clear();
}
