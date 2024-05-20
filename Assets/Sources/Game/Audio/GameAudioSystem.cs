using System;
using Asteroids.Audio;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.Asteroids;
using Asteroids.Game.Enemies;
using Asteroids.Game.GameState;
using Asteroids.Game.Weapons;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game.Audio
{
    public class GameAudioSystem : IInitializeable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly AudioSettings _settings;
        private readonly EventBus<GameOverEvent> _gameOverEvents;
        private readonly EventBus<DestroyModelEvent<AsteroidModel>> _asteroidsDestruction;
        private readonly EventBus<DestroyModelEvent<EnemyModel>> _enemyDestruction;
        private readonly EventBus<ShootEvent> _shootEvents;
        private readonly EventBus<PlayAudioEvent> _audioEvents;

        public GameAudioSystem(
            AudioSettings settings,
            EventBus<GameOverEvent> gameOverEvents, 
            EventBus<DestroyModelEvent<AsteroidModel>> asteroidsDestruction, 
            EventBus<DestroyModelEvent<EnemyModel>> enemyDestruction,
            EventBus<ShootEvent> shootEvents,
            EventBus<PlayAudioEvent> audioEvents
        ) {
            _subscriptions = new DisposableCollection();
            _settings = settings;
            _gameOverEvents = gameOverEvents;
            _asteroidsDestruction = asteroidsDestruction;
            _enemyDestruction = enemyDestruction;
            _shootEvents = shootEvents;
            _audioEvents = audioEvents;
        }

        public void Init()
        {
            _subscriptions.Add(_gameOverEvents.SubscribeTo(OnGameOver));
            _subscriptions.Add(_asteroidsDestruction.SubscribeTo(_ => OnObjectDestroyed()));
            _subscriptions.Add(_enemyDestruction.SubscribeTo(_ => OnObjectDestroyed()));
            _subscriptions.Add(_shootEvents.SubscribeTo(OnShoot));
        }

        private void PlayAudio(AudioClip clip)
        {
            _audioEvents.Push(new PlayAudioEvent(clip));
        }
        
        private void OnShoot(ShootEvent eventData)
        {
            var sound = eventData.Weapon.IsLaser ? _settings.laserShootSound : _settings.regularShootSound;
            PlayAudio(sound);
        }

        private void OnObjectDestroyed()
        {
            PlayAudio(_settings.explosionSound);
        }

        private void OnGameOver(GameOverEvent obj)
        {
            OnObjectDestroyed();
            PlayAudio(_settings.gameOverSound);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}