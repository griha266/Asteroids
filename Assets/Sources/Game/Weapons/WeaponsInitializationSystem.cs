using System;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Utils;

namespace Asteroids.Game.Weapons
{
    public class WeaponsInitializationSystem : IInitializeable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly GameStateModel _gameState;
        private readonly WeaponModel _laser;
        private readonly WeaponModel _regularWeapon;
        private readonly WeaponSettings _laserSettings;
        private readonly WeaponSettings _regularWeaponSettings;
        private readonly IModelsCollection _models;

        public WeaponsInitializationSystem(
            GameStateModel gameState,
            WeaponModel laser,
            WeaponModel regularWeapon,
            IModelsCollection models, 
            WeaponSettings laserSettings, 
            WeaponSettings regularWeaponSettings
        )
        {
            _subscriptions = new DisposableCollection();
            _gameState = gameState;
            _laser = laser;
            _regularWeapon = regularWeapon;
            _models = models;
            _laserSettings = laserSettings;
            _regularWeaponSettings = regularWeaponSettings;
        }

        public void Init()
        {
            _laser.ApplySettings(_laserSettings.reloadDuration, _laserSettings.maxAmmo);
            _models.Add(_laser);
            _regularWeapon.ApplySettings(_regularWeaponSettings.reloadDuration, _regularWeaponSettings.maxAmmo);
            _models.Add(_regularWeapon);
            _subscriptions.Add(_gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged));
        }

        private void OnGameStateChanged(bool isGameLoop)
        {
            _laser.IsEnabled.Value = isGameLoop;
            _regularWeapon.IsEnabled.Value = isGameLoop;
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}