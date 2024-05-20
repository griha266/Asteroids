using System;
using System.Collections.Generic;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Utils;

namespace Asteroids.Game.Weapons
{
    public class WeaponReloadSystem : IInitializeable, IUpdateable, IDisposable
    {
        private IDisposable _subscription;
        private readonly GameStateModel _gameState;
        private readonly IReadOnlyList<WeaponModel> _weapons;
        public WeaponReloadSystem(GameStateModel gameState, IReadOnlyList<WeaponModel> weapons)
        {
            _gameState = gameState;
            _weapons = weapons;
        }

        public void Init()
        {
            _subscription = _gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged);
        }

        private void OnGameStateChanged(bool isGameLoop)
        {
            if (!isGameLoop)
            {
                foreach (var weapon in _weapons)
                {
                    weapon.CurrentReloadTimer.Value = 0;
                    weapon.ResetAmmo();
                }
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var model in _weapons)
            {
                if (!model.IsEnabled.Value)
                {
                    continue;
                }
                
                // do not process full weapon
                if (model.FullMagazine)
                {
                    continue;
                }

                model.CurrentReloadTimer.Value += deltaTime;
                if (model.CurrentReloadTimer.Value >= model.ReloadDuration)
                {
                    model.TryAddAmmo(1);
                    model.CurrentReloadTimer.Value = 0;
                }
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}