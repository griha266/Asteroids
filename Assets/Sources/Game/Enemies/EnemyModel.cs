using Asteroids.Game.Damage;
using Asteroids.Physics;
using Asteroids.Transform;
using UnityEngine;

namespace Asteroids.Game.Enemies
{
    public class EnemyModel : DynamicModel, IDamageable
    {
        public EnemyModel(
            ColliderDescription colliderDescription,
            bool isColliderEnabled,
            float maxVelocity,
            Vector2 initialVelocity,
            Vector2 initialPosition,
            float initialRotation,
            GameTransform parent = null
            ) : base(
            true,
            colliderDescription,
            isColliderEnabled,
            maxVelocity,
            initialVelocity,
            initialPosition,
            initialRotation,
            parent
        ) {
            Health = 1;
        }

        public int Health { get; set; }
    }
}