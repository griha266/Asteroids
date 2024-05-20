using System;
using UnityEngine;
using Asteroids.Core.Systems;
using Asteroids.Utils;

namespace Asteroids.Audio
{
    public class AudioSystem : IInitializeable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly AudioSource _audioPlayer;
        private readonly EventBus<PlayAudioEvent> _audioEvents;

        public AudioSystem(
            AudioSource audioPlayer,
            EventBus<PlayAudioEvent> audioEvents
        ) {
            _audioPlayer = audioPlayer;
            _audioEvents = audioEvents;
            _subscriptions = new DisposableCollection();
        }

        public void Init()
        {
            _subscriptions.Add(_audioEvents.SubscribeTo(OnPlayAudio));
        }

        private void OnPlayAudio(PlayAudioEvent eventData)
        {
            _audioPlayer.PlayOneShot(eventData.Clip);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}