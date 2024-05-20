using Asteroids.Game.Asteroids;
using Asteroids.Game.Enemies;
using Asteroids.Game.Explosion;
using Asteroids.Game.Player;
using Asteroids.Game.Weapons;
using UnityEngine;
using AudioSettings = Asteroids.Game.Audio.AudioSettings;

namespace Asteroids.Game
{
    [CreateAssetMenu(menuName = "Game/Create Game settings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public PlayerSettings playerSettings;
        public EnemySettings enemySettings;
        public AsteroidsSettings asteroidsSettings;
        public ExplosionSettings explosionSettings;
        public WeaponSettings laserSettings;
        public WeaponSettings regularWeaponSettings;
        public AudioSettings audioSettings;
        public float offscreenBorderSize;
    }
}