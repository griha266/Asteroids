using System;
using System.Collections.Generic;

namespace Asteroids.Utils
{
    public class EventBus
    {
        private readonly Dictionary<Type, object> _eventBuses = new();
        
        public EventBus<T> GetBus<T>() where T : struct
        {
            EventBus<T> eventBus;
            var eventType = typeof(T);
            if (_eventBuses.ContainsKey(eventType))
            { 
                eventBus = _eventBuses[eventType] as EventBus<T>;
            }
            else
            {
                eventBus = new EventBus<T>();
                _eventBuses.Add(eventType, eventBus);
            }

            return eventBus;
        }
    }
    
    public class EventBus<T> where T : struct
    {
        private event Action<T> OnEventPushed;

        public void Push(T eventData) 
        {
            OnEventPushed?.Invoke(eventData);
        }

        public IDisposable SubscribeTo(Action<T> onEvent)
        {
            OnEventPushed += onEvent;
            return new DisposeAction(() => OnEventPushed -= onEvent);
        }
    }
}