using System;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Game.Player
{
    public class PlayerInitializationSystem : IInitializeable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly PlayerModel _player;
        private readonly IModelsCollection _models;
        private readonly GameStateModel _gameState;
        private readonly PlayerSettings _settings;

        public PlayerInitializationSystem(IModelsCollection models, PlayerModel player, GameStateModel gameState, PlayerSettings settings)
        {
            _subscriptions = new DisposableCollection();
            _models = models;
            _player = player;
            _gameState = gameState;
            _settings = settings;
        }

        public void Init()
        {
            var view = Object.Instantiate(_settings.prefab);
            var presenter = new ModelToGameObjectPresenter(view.gameObject, _player);
            _models.Add(_player);
            _subscriptions.Add(presenter);
            _subscriptions.Add(_gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged));
        }
        
        private void OnGameStateChanged(bool isGameLoop)
        {
            _player.IsEnabled.Value = isGameLoop;
            // _player.Collider.IsEnabled.Value = isGameLoop;
            if (isGameLoop)
            {
                _player.Transform.WorldPosition.Value = Vector2.zero;
                _player.Transform.WorldRotation.Value = 0;
                _player.Velocity.Value = Vector2.zero;
            }
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}