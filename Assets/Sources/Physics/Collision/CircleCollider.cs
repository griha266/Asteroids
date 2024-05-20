using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Physics
{
    public class CircleCollider
    {
        public readonly LayerMask CurrentLayer;
        public readonly LayerMask CollisionMask;
        public readonly SceneModel Parent;
        public ReactiveProperty<bool> IsEnabled { get; private set; }
        public float Radius { get; }

        public CircleCollider(ColliderDescription colliderDescription, SceneModel parent, bool isEnabled)
        {
            CurrentLayer = colliderDescription.Layer;
            CollisionMask = colliderDescription.CollisionMask;
            Radius = colliderDescription.ColliderRadius;
            Parent = parent;
            IsEnabled = new ReactiveProperty<bool>(isEnabled);
        }
    }
}