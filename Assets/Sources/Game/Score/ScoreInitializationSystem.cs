using System;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game.Score
{
    public class ScoreInitializationSystem : IInitializeable, IDisposable
    {
        private const string MaxScoreKey = "MaxScoreKey";

        private readonly ScoreModel _score;
        private readonly GameStateModel _gameState;
        private readonly EventBus<AddScoreEvent> _addScoreEvents;
        private readonly DisposableCollection _subscription;
        public ScoreInitializationSystem(ScoreModel score, GameStateModel gameState, EventBus<AddScoreEvent> addScoreEvents)
        {
            _score = score;
            _gameState = gameState;
            _addScoreEvents = addScoreEvents;
            _subscription = new DisposableCollection();
        }
        
        public void Init()
        {
            PlayerPrefs.SetInt(MaxScoreKey, 0);
            var maxScore = PlayerPrefs.GetInt(MaxScoreKey, 0);
            _score.MaxScore.Value = maxScore;
            _subscription.Add(_addScoreEvents.SubscribeTo(OnScoreAdded));
            _subscription.Add(_gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged));
        }

        private void OnGameStateChanged(bool isGameLoop)
        {
            if (!isGameLoop)
            {
                _score.ResetCurrentScore();
            }
        }

        private void OnScoreAdded(AddScoreEvent eventData)
        {
            _score.CurrentScore.Value += eventData.AddScoreCount;
            if (_score.CurrentScore.Value > _score.MaxScore.Value)
            {
                _score.MaxScore.Value = _score.CurrentScore.Value;
                PlayerPrefs.SetInt(MaxScoreKey, _score.MaxScore.Value);
            }
        }


        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}