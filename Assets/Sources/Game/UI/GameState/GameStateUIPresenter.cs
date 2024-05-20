using System;
using Asteroids.Game.GameState;
using Asteroids.Game.Player;
using Asteroids.Game.Score;
using Asteroids.Game.Weapons;
using Asteroids.Utils;

namespace Asteroids.Game.UI
{
    public class GameStateUIPresenter : IDisposable
    {
        private readonly DisposableCollection _subscriptions;

        public GameStateUIPresenter(
            GameStateUIView view, 
            WeaponModel laserModel, 
            PlayerModel playerModel, 
            GameStateModel gameStateModel,
            ScoreModel scoreModel
        ) {
            _subscriptions = new DisposableCollection();

            _subscriptions.AddRange(
                gameStateModel.IsGameLoop.SubscribeAndRefresh(view.SetVisibility),
                playerModel.Transform.WorldPosition.SubscribeAndRefresh(view.SetCurrentPositionText),
                playerModel.Transform.WorldRotation.SubscribeAndRefresh(view.SetCurrentRotationText),
                playerModel.Velocity.SubscribeAndRefresh(view.SetCurrentVelocityText),
                scoreModel.CurrentScore.SubscribeAndRefresh(view.SetCurrentScoreText),
                laserModel.CurrentAmmo.SubscribeAndRefresh(currentAmmo => view.SetLaserAmmoText(currentAmmo, laserModel.MaxAmmo)),
                laserModel
                    .CurrentReloadTimer
                    .Map(timer => timer / laserModel.ReloadDuration)
                    .SubscribeAndRefresh(view.SetLaserCharge)
            );
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}