using System;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Game.Player
{
    public class PlayerDamageSystem : IInitializeable, IDisposable
    {
        private readonly EventBus<GameOverEvent> _gameOverEvent;
        private readonly EventBus<CollisionEvent> _collisionEvents;
        private IDisposable _subscription;
        
        public PlayerDamageSystem(EventBus<GameOverEvent> gameOverEvents, EventBus<CollisionEvent> collisionEvents)
        {
            _gameOverEvent = gameOverEvents;
            _collisionEvents = collisionEvents;
        }

        public void Init()
        {
            _subscription = _collisionEvents.SubscribeTo(OnCollision);
        }

        private void OnCollision(CollisionEvent collisionEvent)
        {
            var firstModel = collisionEvent.Collision.FirstCollider.Parent;
            var secondModel = collisionEvent.Collision.SecondCollider.Parent;
            if (firstModel is PlayerModel || secondModel is PlayerModel)
            {
                _gameOverEvent.Push(new GameOverEvent());
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}