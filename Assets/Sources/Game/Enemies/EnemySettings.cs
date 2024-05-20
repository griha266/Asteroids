using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Enemies
{
    [CreateAssetMenu(menuName = "Game/Create Enemy settings", fileName = "EnemySettings", order = 0)]
    public class EnemySettings : ScriptableObject
    {
        public ColliderDescriptionComponent enemyPrefab;
        public float spawnDuration;
        public int maxEnemiesCount;
        public float maxVelocity;
        public float maxAvoidanceForce;
        public float avoidDistance;
    }
}