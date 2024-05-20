using System;
using System.Collections.Generic;

namespace Asteroids.Utils
{
    public class ObjectPool<T> 
        where T : class 
    {
        private readonly Stack<T> _pool;
        private readonly Func<T> _instanceBuilder;

        public ObjectPool(Func<T> instanceBuilder, int preloadCount = 0)
        {
            _pool = new Stack<T>();
            _instanceBuilder = instanceBuilder;
            for (int i = 0; i < preloadCount; i++)
            {
                _pool.Push(_instanceBuilder());
            }
        }

        public T Get()
        {
            return _pool.TryPop(out var result) ? result : _instanceBuilder();
        }

        public void Release(T instance)
        {
            _pool.Push(instance);
        }

    }
}