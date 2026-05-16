using System;

namespace PhikozzLib
{
    public interface IEventService
    {
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
        void Publish<T>(T evt);
        void Clear();
    }
}