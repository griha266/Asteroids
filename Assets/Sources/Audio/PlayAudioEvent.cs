using UnityEngine;

namespace Asteroids.Audio
{
    public readonly struct PlayAudioEvent
    {
        public readonly AudioClip Clip;

        public PlayAudioEvent(AudioClip clip)
        {
            Clip = clip;
        }
    }
}
