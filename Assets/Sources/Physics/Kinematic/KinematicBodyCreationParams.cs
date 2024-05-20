using UnityEngine;

namespace Asteroids.Physics
{
    public readonly struct KinematicBodyCreationParams
    {
        public readonly Vector2 Position;
        public readonly float Rotation;
        public readonly Vector2 Velocity;

        public KinematicBodyCreationParams(Vector2 position, float rotation, Vector2 velocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
        }

    }
}