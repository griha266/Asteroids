using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Physics;
using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Game.Asteroids
{
    public class AsteroidSpawnSystem : IInitializeable, IUpdateable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly AsteroidsSettings _settings;
        private readonly GameSceneSize _sceneSize;
        private readonly IReadOnlyList<AsteroidModel> _asteroids;
        private readonly IModelsCollection _modelsCollection;
        private readonly GameStateModel _gameState;
        private readonly EventBus<DestroyModelEvent<AsteroidModel>> _destroyAsteroidsEvents;
        private AsteroidFactory _bigFactory;
        private AsteroidFactory _smallFactory;

        private float _spawnTimer;

        public AsteroidSpawnSystem(
            GameSceneSize sceneSize, 
            AsteroidsSettings settings,
            IReadOnlyList<AsteroidModel> asteroids,
            IModelsCollection modelsCollection,
            EventBus<DestroyModelEvent<AsteroidModel>> destroyAsteroidsEvents, GameStateModel gameState) {
            _subscriptions = new DisposableCollection();
            _settings = settings;
            _sceneSize = sceneSize;
            _asteroids = asteroids;
            _modelsCollection = modelsCollection;
            _destroyAsteroidsEvents = destroyAsteroidsEvents;
            _gameState = gameState;
        }
        
        public void Init()
        {
            _subscriptions.Add(_gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged));
            _bigFactory = CreateFactory(true, _modelsCollection);
            _smallFactory = CreateFactory(false, _modelsCollection);
            _subscriptions.Add(_destroyAsteroidsEvents.SubscribeTo(OnAsteroidDestroyed));
            _subscriptions.Add(_bigFactory);
            _subscriptions.Add(_smallFactory);
        }

        private void OnAsteroidDestroyed(DestroyModelEvent<AsteroidModel> eventData)
        {
            if (eventData.Model.IsBig)
            {
                if (eventData.Model.Health == 1)
                {
                    SpawnSmallAsteroids(eventData.Model);
                }
            }
            var factory = eventData.Model.IsBig ? _bigFactory : _smallFactory;
            factory.Destroy(eventData.Model);
        }

        private void SpawnSmallAsteroids(AsteroidModel bigAsteroid)
        {
            var position = bigAsteroid.Transform.WorldPosition.Value;
            var up = bigAsteroid.Transform.Up();
            for (int i = 0; i < _settings.smallAsteroidsCount; i++)
            {
                var rotateOffset = Random.Range(0, 360);
                var positionOffset = Random.Range(0.3f, 0.5f);
                var spawnPoint = position + MathUtils.RotatePoint(up, rotateOffset) * positionOffset;
                var spawnRotation = Random.Range(0, 360);
                var spawnVelocity = Random.Range(_settings.minSpawnVelocity, _settings.maxSpawnVelocity);
                _smallFactory.Create(
                    new KinematicBodyCreationParams(
                        spawnPoint,
                        spawnRotation,
                        MathUtils.RotatePoint(Vector2.up, spawnRotation) * spawnVelocity
                    )
                );
            }
        }

        private AsteroidFactory CreateFactory(bool isBig, IModelsCollection models)
        {
            var prefab = isBig ? _settings.bigAsteroidPrefab : _settings.smallAsteroidPrefab;
            var factory = new AsteroidFactory(isBig, _settings.maxAvailableVelocity, models, prefab);
            factory.Init();
            return factory;
        }

        public void Update(float deltaTime)
        {
            if (_gameState.IsGameLoop.Value)
            {
                var modelsCount = _asteroids.Count(asteroid => asteroid.IsBig);
                if (modelsCount < _settings.maxBigAsteroidsCount)
                {
                    _spawnTimer += deltaTime;
                    if (_spawnTimer >= _settings.spawnDelay)
                    {
                        _spawnTimer = 0;
                        CreateAsteroid();
                    }
                }
            }
        }

        private void CreateAsteroid()
        {
            var creationParams = PhysicsUtils.CreateRandomSpawnParams(
                _sceneSize, 
                _settings.minSpawnVelocity, 
                _settings.maxSpawnVelocity
            );
            _bigFactory.Create(creationParams);
        }
        
        private void OnGameStateChanged(bool isGameLoop)
        {
            if (!isGameLoop)
            {
                foreach (var asteroid in _asteroids)
                {
                    var factory = asteroid.IsBig ? _bigFactory : _smallFactory;
                    factory.Destroy(asteroid);
                }
            }
        }
        
        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}