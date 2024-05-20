using System;
using System.Collections.Generic;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Physics;
using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game.Enemies
{
    public class EnemySpawnSystem : IInitializeable, IUpdateable, IDisposable
    {
        private readonly GameStateModel _gameState;
        private readonly DisposableCollection _subscriptions;
        private readonly IReadOnlyList<EnemyModel> _enemies;
        private readonly IModelsCollection _collection;
        private readonly EnemySettings _settings;
        private readonly EventBus<DestroyModelEvent<EnemyModel>> _destroyEnemiesEvents;
        private readonly GameSceneSize _sceneSize;
        
        private EnemyFactory _factory;
        private float _timer;

        public EnemySpawnSystem(GameStateModel gameState,
            GameSceneSize sceneSize,
            IReadOnlyList<EnemyModel> enemies,
            IModelsCollection collection,
            EnemySettings settings, 
            EventBus<DestroyModelEvent<EnemyModel>> destroyEnemiesEvents
        )
        {
            _gameState = gameState;
            _subscriptions = new DisposableCollection();
            _enemies = enemies;
            _collection = collection;
            _sceneSize = sceneSize;
            _settings = settings;
            _destroyEnemiesEvents = destroyEnemiesEvents;
        }

        public void Init()
        {
            _factory = new EnemyFactory(_settings, _collection, 4);
            _factory.Init();
            _subscriptions.Add(_factory);
            _subscriptions.Add(_gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged));
            _subscriptions.Add(_destroyEnemiesEvents.SubscribeTo(OnModelDestroyed));
        }

        private void OnModelDestroyed(DestroyModelEvent<EnemyModel> eventData)
        {
            _factory.Destroy(eventData.Model);
        }

        public void Update(float deltaTime)
        {
            if (!_gameState.IsGameLoop.Value)
            {
                return;
            }
            if (_enemies.Count >= _settings.maxEnemiesCount)
            {
                return;
            }

            _timer += deltaTime;
            if (_timer >= _settings.spawnDuration)
            {
                _timer = 0;
                var randomPosition = GameTransformUtils.GetRandomSceneBorderPosition(_sceneSize);
                var creationParams = new KinematicBodyCreationParams(randomPosition, 0, Vector2.zero);
                _factory.Create(creationParams);
            }
        }


        public void Dispose()
        {
            _subscriptions.Dispose();
        }


        private void OnGameStateChanged(bool isGameLoop)
        {
            if (!isGameLoop)
            {
                foreach (var enemy in _enemies)
                {
                    _factory.Destroy(enemy);
                }
            }
        }
    }
}