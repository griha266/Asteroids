using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.Asteroids;
using Asteroids.Game.Enemies;
using Asteroids.Game.GameState;
using Asteroids.Game.Player;
using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game.Explosion
{
    public class ExplosionSystem : IInitializeable, IDisposable, IUpdateable
    {
        private GameObjectPool _pool;
        private readonly DisposableCollection _subscriptions;
        private readonly PlayerModel _player;
        private readonly GameObject _prefab;
        private readonly float _appearDuration;
        private readonly EventBus<DestroyModelEvent<AsteroidModel>> _asteroidsDestroyEvents;
        private readonly EventBus<DestroyModelEvent<EnemyModel>> _enemyDestroyedEvents;
        private readonly EventBus<GameOverEvent> _gameOverEvents;
        private readonly Dictionary<GameObject, float> _activeObjects;
        private readonly List<GameObject> _toClear = new();

        public ExplosionSystem(
            PlayerModel player,
            GameObject prefab, 
            float appearDuration, 
            EventBus<DestroyModelEvent<AsteroidModel>> asteroidsDestroyEvents, 
            EventBus<DestroyModelEvent<EnemyModel>> enemyDestroyedEvents, 
            EventBus<GameOverEvent> gameOverEvents
        )
        {
            _player = player;
            _prefab = prefab;
            _appearDuration = appearDuration;
            _asteroidsDestroyEvents = asteroidsDestroyEvents;
            _enemyDestroyedEvents = enemyDestroyedEvents;
            _gameOverEvents = gameOverEvents;
            _subscriptions = new DisposableCollection();
            _activeObjects = new Dictionary<GameObject, float>();
        }

        public void Init()
        {
            _subscriptions.Add(_asteroidsDestroyEvents.SubscribeTo(data => OnSceneModelDestroyed(data.Model)));
            _subscriptions.Add(_enemyDestroyedEvents.SubscribeTo(data => OnSceneModelDestroyed(data.Model)));
            _subscriptions.Add(_gameOverEvents.SubscribeTo(OnGameOver));
            _pool = new GameObjectPool(_prefab, 4, "Explosion");
            _subscriptions.Add(_pool);
        }

        private void OnGameOver(GameOverEvent obj)
        {
            OnSceneModelDestroyed(_player);
        }

        private void OnSceneModelDestroyed(SceneModel model)
        {
            var view = _pool.Get(isActiveOnReceiving: true);
            view.transform.position = model.Transform.WorldPosition.Value;
            view.transform.rotation = Quaternion.Euler(0, 0, model.Transform.WorldRotation.Value);
            _activeObjects.Add(view, 0);
        }


        public void Dispose()
        {
            foreach (var pair in _activeObjects)
            {
                _pool.Release(pair.Key);
            }
            _activeObjects.Clear();
            _subscriptions.Dispose();
        }


        public void Update(float deltaTime)
        {
            var keys = _activeObjects.Keys.ToList();
            foreach (var key in keys)
            {
                _activeObjects[key] += deltaTime;
                if (_activeObjects[key] > _appearDuration)
                {
                    _toClear.Add(key);
                }
            }

            foreach (var obj in _toClear)
            {
                _activeObjects.Remove(obj);
                _pool.Release(obj);
            }
            _toClear.Clear();
        }

    }
}