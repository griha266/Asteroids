using System;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Game.Player;
using Asteroids.Game.Score;
using Asteroids.Game.Weapons;
using Asteroids.Utils;

namespace Asteroids.Game.UI
{
    public class UIInitializationSystem : IInitializeable, IDisposable
    {
        private readonly ApplicationUI _ui;
        private readonly PlayerModel _playerModel;
        private readonly WeaponModel _laserWeapon;
        private readonly GameStateModel _gameState;
        private readonly ScoreModel _scoreModel;
        private readonly DisposableCollection _subscriptions;

        public UIInitializationSystem(
            ApplicationUI ui,
            PlayerModel playerModel,
            WeaponModel laserWeapon,
            GameStateModel gameState,
            ScoreModel scoreModel
        ) {
            _ui = ui;
            _playerModel = playerModel;
            _gameState = gameState;
            _scoreModel = scoreModel;
            _laserWeapon = laserWeapon;
            _subscriptions = new DisposableCollection();
        }

        public void Init()
        {
            var gameStatePresenter = new GameStateUIPresenter(_ui.gameUi, _laserWeapon, _playerModel, _gameState, _scoreModel);
            _subscriptions.Add(gameStatePresenter);
            var mainUIPresenter = new MainUIViewPresenter(_scoreModel, _gameState, _ui.mainUI);
            _subscriptions.Add(mainUIPresenter);
        }
        
        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}