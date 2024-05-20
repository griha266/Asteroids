using System.Collections.Generic;
using Asteroids.Core.Systems;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Physics
{
    public class CollisionDetectionSystem : IUpdateable
    {
        private readonly HashSet<Collision> _currentCollisions;
        private readonly EventBus<CollisionEvent> _collisionEventBus;
        private readonly IReadOnlyList<IWithCollider> _models;
        
        public CollisionDetectionSystem(EventBus<CollisionEvent> collisionEvents, IReadOnlyList<IWithCollider> models)
        {
            _currentCollisions = new HashSet<Collision>();
            _collisionEventBus = collisionEvents;
            _models = models;
        }

        public void Update(float deltaTime)
        {
            foreach (var firstModel in _models)
            {
                if (!firstModel.IsEnabled.Value)
                {
                    continue;
                }
                foreach (var secondModel in _models)
                {
                    if (!secondModel.IsEnabled.Value || firstModel == secondModel)
                    {
                        continue;
                    }

                    var firstCollider = firstModel.Collider;
                    var secondCollider = secondModel.Collider;
                    
                    if (!firstCollider.IsEnabled.Value || !secondCollider.IsEnabled.Value)
                    {
                        continue;
                    }
                    
                    if (!firstCollider.CurrentLayer.IsInsideMask(secondCollider.CollisionMask) || !secondCollider.CurrentLayer.IsInsideMask(firstCollider.CollisionMask))
                    {
                        continue;
                    }
                    
                    
                    var possibleCollision = new Collision(firstCollider, secondCollider);
                    if (_currentCollisions.Contains(possibleCollision))
                    {
                        // Do not process existing collisions
                        continue;
                    }

                    
                    if (IsColliding(firstCollider, secondCollider, out var displacement))
                    {
                        _currentCollisions.Add(possibleCollision);
                        _collisionEventBus.Push(new CollisionEvent(possibleCollision,displacement));
                    }

                }
            }
            _currentCollisions.Clear();
        }

        private static bool IsColliding(
            CircleCollider firstCircle, 
            CircleCollider secondCircle,
            out Vector2 displacement
        ) {
            displacement = Vector2.zero;
            var firstPosition = firstCircle.Parent.Transform.WorldPosition.Value;
            var secondPosition = secondCircle.Parent.Transform.WorldPosition.Value;
            var combinedRadius = firstCircle.Radius + secondCircle.Radius;
            var distance = secondPosition - firstPosition;
            if (distance.sqrMagnitude <= combinedRadius * combinedRadius)
            {
                var distanceMagnitude = distance.magnitude;
                var intersectionDistance = combinedRadius - distanceMagnitude;
                displacement = distance.normalized * (intersectionDistance * 0.5f);
                return true;
            }

            return false;
        }
    }
}