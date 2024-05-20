using UnityEngine;

namespace Asteroids.Game.Audio
{
    [CreateAssetMenu(menuName = "Game/Create Audio settings", fileName = "AudioSettings", order = 0)]
    public class AudioSettings : ScriptableObject
    {
        public AudioClip gameOverSound;
        public AudioClip explosionSound;
        public AudioClip regularShootSound;
        public AudioClip laserShootSound;
    }
}