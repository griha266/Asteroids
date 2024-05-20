using System;

namespace Asteroids.Utils
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _callback;
        private bool _disposed;
        public DisposeAction(Action callback)
        {
            _callback = callback;
            _disposed = false;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _callback?.Invoke();
        }
    }
}