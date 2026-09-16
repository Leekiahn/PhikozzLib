using System;
using UnityEngine;

namespace PhikozzLib
{
    public interface IEventService
    {
        void Subscribe<T>(Action<T> handler) where T : struct;
        void Subscribe<T>(Component owner, Action<T> handler) where T : struct;
        void Unsubscribe<T>(Action<T> handler) where T : struct;
        void Publish<T>(T evt) where T : struct;
        void Clear();
    }
}