using UnityEngine;

namespace Asteroids.Physics
{
    public readonly struct CollisionEvent
    {
        public readonly Collision Collision;
        public readonly Vector2 ResolveVector;

        public CollisionEvent(Collision collision, Vector2 resolveVector)
        {
            Collision = collision;
            ResolveVector = resolveVector;
        }
    }
}