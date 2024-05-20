using System;
using Asteroids.Game.GameState;
using Asteroids.Game.Score;
using Asteroids.Utils;

namespace Asteroids.Game.UI
{
    public class MainUIViewPresenter : IDisposable
    {
        private readonly MainUIView _view;
        private readonly DisposableCollection _subscriptions;
        
        public MainUIViewPresenter(ScoreModel scoreModel, GameStateModel gameStateModel, MainUIView view)
        {
            _view = view;
            _subscriptions = new DisposableCollection();
            
            var scoreSubscription = scoreModel.MaxScore.SubscribeAndRefresh(OnMaxScoreChanged);
            _subscriptions.Add(scoreSubscription);

            var gameStateSubscription = gameStateModel.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged);
            _subscriptions.Add(gameStateSubscription);
        }

        private void OnGameStateChanged(bool isGameLoop)
        {
            _view.SetVisibility(!isGameLoop);
        }

        private void OnMaxScoreChanged(int maxScore)
        {
            _view.SetMaxScore(maxScore);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}