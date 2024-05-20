using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Physics
{
    public interface IWithVelocity : IWithTransform
    {
        float MaxVelocity { get; }
        IReactiveProperty<Vector2> Velocity { get; }
    }

}