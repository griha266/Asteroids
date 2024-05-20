using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Weapons
{
    public class BulletModel : DynamicModel
    {
        public WeaponModel ParentWeapon { get; }
        public bool DestroyOnCollision { get; }
        
        public BulletModel(
            WeaponModel parentWeapon,
            bool destroyOnCollision,
            ColliderDescription colliderDescription,
            bool isEnabled 
        ) : base(
            false,
            colliderDescription,
            isEnabled,
            float.MaxValue, 
            Vector2.zero,
            parentWeapon.Transform.WorldPosition.Value, 
            parentWeapon.Transform.WorldRotation.Value
        ) {
            ParentWeapon = parentWeapon;
            DestroyOnCollision = destroyOnCollision;
        }
    }
}