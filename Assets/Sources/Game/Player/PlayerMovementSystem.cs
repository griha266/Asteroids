using System;
using Asteroids.Core.Systems;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game.Player
{
    public class PlayerMovementSystem : IInitializeable, IDisposable
    {
        private readonly PlayerModel _player;
        private readonly DisposableCollection _subscriptions;
        private readonly float _acceleration;
        private readonly float _rotationSpeed;
        private readonly EventBus<AcceleratePlayerEvent> _accelerateEvents;
        private readonly EventBus<RotatePlayerEvent> _rotateEvents;

        public PlayerMovementSystem(
            PlayerModel player,
            float acceleration,
            float rotationSpeed,
            EventBus<AcceleratePlayerEvent> accelerateEvents,
            EventBus<RotatePlayerEvent> rotateEvents
        )
        {
            _subscriptions = new DisposableCollection();
            _player = player;
            _acceleration = acceleration;
            _rotationSpeed = rotationSpeed;
            _accelerateEvents = accelerateEvents;
            _rotateEvents = rotateEvents;
        }

        public void Init()
        {
            _subscriptions.Add(_accelerateEvents.SubscribeTo(OnAccelerate));
            _subscriptions.Add(_rotateEvents.SubscribeTo(OnRotate));
        }
        

        private void OnRotate(RotatePlayerEvent eventData)
        {
            if(_player.IsEnabled.Value)
            {
                var rotateValue = eventData.Value * _rotationSpeed;
                _player.Transform.WorldRotation.Value += rotateValue * eventData.DeltaTime;
            }
        }

        private void OnAccelerate(AcceleratePlayerEvent eventData)
        {
            if (_player.IsEnabled.Value)
            {
                var inputValue = eventData.Value;
                var scaledAcceleration = inputValue * _acceleration;
                var worldUp = _player.Transform.Up();
                var additionalVelocity = worldUp * (scaledAcceleration * eventData.DeltaTime);
                _player.Velocity.Value += additionalVelocity;
            }
        }


        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}