using System;

namespace Asteroids.Utils
{
    public static class ReactivePropertyUtils
    {
        public static IDisposable SubscribeAndRefresh<T>(this IReadOnlyReactiveProperty<T> property, Action<T> callback)
        {
            property.OnValueChanged += callback;
            callback.Invoke(property.Value);
            return new DisposeAction(() => property.OnValueChanged -= callback);
        }

        public static IReadOnlyReactiveProperty<TResult> Map<TSource, TResult>(
            this IReadOnlyReactiveProperty<TSource> source,
            Func<TSource, TResult> mapper
        )
        {
            return new ReadOnlyMapReactiveProperty<TSource, TResult>(source, mapper);
        }

        public static IReactiveProperty<TResult> MapTwoWay<TSource, TResult>(
            this IReactiveProperty<TSource> source,
            Func<TSource, TResult> getter,
            Func<TResult, TSource> setter
        )
        {
            return new TwoWayMappedReactiveProperty<TSource, TResult>(source, getter, setter);
        }
    }
}