using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Asteroids
{
    [CreateAssetMenu(menuName = "Game/Create Asteroids settings", fileName = "AsteroidSettings", order = 0)]
    public class AsteroidsSettings : ScriptableObject
    {
        public ColliderDescriptionComponent bigAsteroidPrefab;
        public ColliderDescriptionComponent smallAsteroidPrefab;
        public float spawnDelay;
        public float minSpawnVelocity;
        public float maxSpawnVelocity;
        public float maxAvailableVelocity;
        public int maxBigAsteroidsCount;
        public int smallAsteroidsCount;
    }
}