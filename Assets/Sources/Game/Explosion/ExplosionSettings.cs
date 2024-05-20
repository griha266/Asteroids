using UnityEngine;

namespace Asteroids.Game.Explosion
{
    [CreateAssetMenu(menuName = "Game/Create Explosion settings", fileName = "ExplosionSettings", order = 0)]
    public class ExplosionSettings : ScriptableObject
    {
        public GameObject prefab;
        public float duration;
    }
}