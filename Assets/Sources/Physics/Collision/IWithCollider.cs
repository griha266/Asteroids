namespace Asteroids.Physics
{
    public interface IWithCollider : IWithVelocity
    {
        CircleCollider Collider { get; }
    }
}