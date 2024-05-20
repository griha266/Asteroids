using System;

namespace Asteroids.Utils
{
    public interface IReadOnlyReactiveProperty<out T>
    {
        T Value { get; }
        event Action<T> OnValueChanged;
    }

    public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        new T Value { get; set; }
    }

    public class TwoWayMappedReactiveProperty<TSource, TResult> : IReactiveProperty<TResult>, IDisposable
    {
        private readonly IReactiveProperty<TSource> _source;
        private readonly Func<TSource, TResult> _getter;
        private readonly Func<TResult, TSource> _setter;
        private TResult _previousValue;
        
        public TResult Value
        {
            get => _getter(_source.Value);
            set => _source.Value = _setter(value);
        }

        public event Action<TResult> OnValueChanged;

        public TwoWayMappedReactiveProperty(
            IReactiveProperty<TSource> source, 
            Func<TSource, TResult> getter, 
            Func<TResult, TSource> setter
        ) {
            _source = source;
            _getter = getter;
            _setter = setter;
            _previousValue = Value;
            _source.OnValueChanged += OnSourceChanged;
        }

        public void Dispose()
        {
            _source.OnValueChanged -= OnSourceChanged;
        }

        private void OnSourceChanged(TSource value)
        {
            if (Equals(value, _previousValue))
            {
                return;
            }

            _previousValue = _getter(value);
            OnValueChanged?.Invoke(_previousValue);
        }
    }

    public class ReadOnlyMapReactiveProperty<TSource, TResult> : IReadOnlyReactiveProperty<TResult>, IDisposable
    {
        private readonly IReadOnlyReactiveProperty<TSource> _source;
        private readonly Func<TSource, TResult> _getter;
        private TResult _previousValue;
        public TResult Value => _getter(_source.Value); 
        public event Action<TResult> OnValueChanged;

        public ReadOnlyMapReactiveProperty(IReadOnlyReactiveProperty<TSource> source, Func<TSource, TResult> getter)
        {
            _source = source;
            _getter = getter;
            _previousValue = Value;
            _source.OnValueChanged += OnSourceChanged;
        }

        private void OnSourceChanged(TSource sourceValue)
        {
            if (Equals(_previousValue, sourceValue))
            {
                return;
            }

            _previousValue = _getter(sourceValue);
            OnValueChanged?.Invoke(_previousValue);
        }

        public void Dispose()
        {
            _source.OnValueChanged -= OnSourceChanged;
        }
    }
    
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                {
                    return;
                }

                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        public event Action<T> OnValueChanged;

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }
        
    }
}