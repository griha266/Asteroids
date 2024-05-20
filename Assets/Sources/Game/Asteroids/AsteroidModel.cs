using Asteroids.Game.Damage;
using Asteroids.Physics;
using Asteroids.Transform;
using UnityEngine;

namespace Asteroids.Game.Asteroids
{
    public class AsteroidModel : DynamicModel, IDamageable
    {
        public bool IsBig { get; }

        public int Health { get; set; }

        public AsteroidModel(
            bool isBig,
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
        )
        {
            IsBig = isBig;
            Health = isBig ? 2 : 1;
        }
    }
}