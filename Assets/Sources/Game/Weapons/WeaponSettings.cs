using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Weapons
{
    [CreateAssetMenu(menuName = "Game/Create Weapon settings", fileName = "WeaponSettings", order = 0)]
    public class WeaponSettings : ScriptableObject
    {
        public float reloadDuration;
        public int maxAmmo;
        public float spawnBulletVelocity;
        public int damage;
        public ColliderDescriptionComponent bulletPrefab;
        public bool destroyBulletsOnCollision;
    }
}