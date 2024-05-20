using System;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Transform
{
    public abstract class SceneFactoryBase<TModel, TCreationParams> : IDisposable, IInitializeable
        where TModel : SceneModel
        where TCreationParams : struct
    {
        protected DisposableCollection Subscriptions { get; }
        private readonly GameObject _viewPrefab;
        private readonly IModelsCollection _modelRegistry;
        private readonly int _preloadedModelsCount;
        private ObjectPool<TModel> _modelsPool;

        protected SceneFactoryBase(GameObject viewPrefab, IModelsCollection modelRegistry, int preloadedModelsCount = 0)
        {
            Subscriptions = new DisposableCollection();
            _viewPrefab = viewPrefab;
            _modelRegistry = modelRegistry;
            _preloadedModelsCount = preloadedModelsCount;
        }

        protected abstract TModel BuildModelInstance();
        protected virtual void OnModelGet(TModel model, TCreationParams creationParams) { }

        protected virtual void OnModelRelease(TModel model) { }

        public void Init()
        {
            _modelsPool = new ObjectPool<TModel>(CreateModel, _preloadedModelsCount);
        }
        private TModel CreateModel()
        {
            var instance = BuildModelInstance();
            var view = Object.Instantiate(_viewPrefab);
            var presenter = new ModelToGameObjectPresenter(view, instance);
            Subscriptions.Add(presenter);
            return instance;
        }

        public TModel Create(TCreationParams creationParams)
        {
            var instance = _modelsPool.Get();
            OnModelGet(instance, creationParams);
            _modelRegistry.Add(instance);
            return instance;
        }

        public void Destroy(TModel instance)
        {
            _modelRegistry.Remove(instance);
            OnModelRelease(instance);
            _modelsPool.Release(instance);
        }

        public void Dispose()
        {
            OnDispose();
            Subscriptions.Dispose();
        }

        protected virtual void OnDispose() { }
    }
}