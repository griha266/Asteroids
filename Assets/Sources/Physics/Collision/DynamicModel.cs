using Asteroids.Transform;
using UnityEngine;

namespace Asteroids.Physics
{
    public abstract class DynamicModel : KinematicModel, IWithCollider
    {
        public bool AffectedByImpulse { get; }
        public CircleCollider Collider { get; }
        
        protected DynamicModel(
            bool affectedByImpulse,
            ColliderDescription colliderDescription,
            bool isColliderEnabled,
            float maxVelocity, 
            Vector2 initialVelocity, 
            Vector2 initialPosition, 
            float initialRotation, 
            GameTransform parent = null
        ) : base(maxVelocity, initialVelocity, initialPosition, initialRotation, parent)
        {
            AffectedByImpulse = affectedByImpulse;
            Collider = new CircleCollider(colliderDescription,this, isColliderEnabled);
        }

    }
}