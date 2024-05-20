using System;
using System.Collections.Generic;

namespace Asteroids.Utils
{
    public class DisposableCollection : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();
        
        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void AddRange(params IDisposable[] disposables)
        {
            _disposables.AddRange(disposables);
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }
    }
}