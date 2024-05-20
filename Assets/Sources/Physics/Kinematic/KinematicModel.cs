using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Physics
{
    public abstract class KinematicModel : SceneModel, IWithVelocity
    {
        public float MaxVelocity { get; }
        public IReactiveProperty<Vector2> Velocity { get; }
        
        protected KinematicModel(
            float maxVelocity, 
            Vector2 initialVelocity,
            Vector2 initialPosition, 
            float initialRotation, 
            GameTransform parent = null
        ) : base(initialPosition, initialRotation, parent)
        {
            MaxVelocity = maxVelocity;
            Velocity = new ReactiveProperty<Vector2>(initialVelocity);
        }
    }
}