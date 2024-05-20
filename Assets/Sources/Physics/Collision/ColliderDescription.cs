using System;
using UnityEngine;

namespace Asteroids.Physics
{
    [Serializable] 
    public struct ColliderDescription
    {
        public float ColliderRadius;
        public LayerMask Layer;
        public LayerMask CollisionMask;
    }
}