using System;
using Asteroids.Core.Systems;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Physics
{
    public class DynamicModelsCollisionSystem : IInitializeable, IDisposable
    {
        private readonly EventBus<CollisionEvent> _collisionEvents;
        private readonly DisposableCollection _subscriptions;
        
        public DynamicModelsCollisionSystem(EventBus<CollisionEvent> collisionEvents)
        {
            _collisionEvents = collisionEvents;
            _subscriptions = new DisposableCollection();
        }
        
        public void Init()
        {
            _subscriptions.Add(_collisionEvents.SubscribeTo(OnCollisionEvent));
        }

        private static void OnCollisionEvent(CollisionEvent collisionEvent)
        {
            var firstModel = collisionEvent.Collision.FirstCollider.Parent;
            var secondModel = collisionEvent.Collision.SecondCollider.Parent;
            
            if (firstModel is DynamicModel firstDynamicModel && secondModel is DynamicModel secondDynamicModel
                && firstDynamicModel.AffectedByImpulse && secondDynamicModel.AffectedByImpulse
            )
            {
                // Collision resolving
                var displacement = collisionEvent.ResolveVector;
                var firstPosition = firstModel.Transform.WorldPosition;
                var secondPosition = secondModel.Transform.WorldPosition;
                firstPosition.Value -= displacement;
                secondPosition.Value += displacement;
                
                // Calculate impulse
                var diffVector = firstPosition.Value - secondPosition.Value;
                var localCollisionPoint = diffVector * 0.5f;
                var collisionNormal = localCollisionPoint.normalized;
                var relativeVelocity = firstDynamicModel.Velocity.Value - secondDynamicModel.Velocity.Value;
                // perfect elastic and masses are the same for everyone
                var impulse = -Vector2.Dot(relativeVelocity, collisionNormal);
                firstDynamicModel.Velocity.Value += impulse * collisionNormal;
                secondDynamicModel.Velocity.Value -= impulse * collisionNormal;
            }
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}