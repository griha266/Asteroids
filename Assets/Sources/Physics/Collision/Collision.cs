using System;

namespace Asteroids.Physics
{
    public readonly struct Collision : IEquatable<Collision>
    {
        public readonly CircleCollider FirstCollider;
        public readonly CircleCollider SecondCollider;

        public Collision(CircleCollider firstCollider, CircleCollider secondCollider)
        {
            FirstCollider = firstCollider;
            SecondCollider = secondCollider;
        }

        public override bool Equals(object obj)
        {
            return obj is Collision other && Equals(other);
        }

        public bool Equals(Collision other)
        {
            return (FirstCollider == other.FirstCollider && SecondCollider == other.SecondCollider) ||
                   (FirstCollider == other.SecondCollider && SecondCollider == other.FirstCollider);
        }

        public override int GetHashCode()
        {
            var firstHash = FirstCollider?.GetHashCode() ?? 0;
            var secondHash = SecondCollider?.GetHashCode() ?? 0;

            // Use xor to prevent hash collisions (4 + 5 == 2 + 7)
            return firstHash ^ secondHash + firstHash + secondHash;
        }
    }
}