using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Game/CreateResourceAsset", fileName = "GameResources", order = 0)]
    public class GameResources : ScriptableObject
    {
        [SerializeField] public GameObject PlayerViewPrefab;
        [SerializeField] public GameObject EnemyViewPrefab;
        [SerializeField] public GameObject BigAsteroidViewPrefab;
        [SerializeField] public GameObject SmallAsteroidViewPrefab;
        [SerializeField] public GameObject GameUIViewPrefab;
        [SerializeField] public GameObject ExplosionViewPrefab;
        [SerializeField] public GameObject BulletViewPrefab;
        [SerializeField] public GameObject LaserRayViewPrefab;
    }
}