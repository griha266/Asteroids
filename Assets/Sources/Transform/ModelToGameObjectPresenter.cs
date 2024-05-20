using System;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Transform
{
    public class ModelToGameObjectPresenter : IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly GameObject _gameObject;

        public ModelToGameObjectPresenter(GameObject gameObject, IWithTransform model)
        {
            _subscriptions = new DisposableCollection();
            _gameObject = gameObject;
            _subscriptions.Add(model.Transform.WorldPosition.SubscribeAndRefresh(OnModelWorldPositionChanged));
            _subscriptions.Add(model.Transform.WorldRotation.SubscribeAndRefresh(OnModelWorldRotationChanged));
            _subscriptions.Add(model.IsEnabled.SubscribeAndRefresh(OnModelVisibilityChanged));
        }

        private void OnModelWorldPositionChanged(Vector2 worldPosition)
        {
            _gameObject.transform.position = worldPosition;
        }

        private void OnModelWorldRotationChanged(float worldDegrees)
        {
            _gameObject.transform.rotation = Quaternion.Euler(0,0,worldDegrees);
        }

        private void OnModelVisibilityChanged(bool isVisible)
        {
            _gameObject.SetActive(isVisible);
        }
        
        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}