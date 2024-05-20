using System;
using Asteroids.Core.Systems;
using Asteroids.Utils;

namespace Asteroids.Game.GameState
{
    public class GameStateInitialization : IInitializeable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly GameStateModel _gameState;
        private readonly EventBus<GameOverEvent> _gameOverEvent;
        private readonly EventBus<StartGameEvent> _startGameEvents;

        public GameStateInitialization(GameStateModel gameState, EventBus<GameOverEvent> gameOverEvent, EventBus<StartGameEvent> startGameEvents)
        {
            _subscriptions = new DisposableCollection();
            _gameState = gameState;
            _gameOverEvent = gameOverEvent;
            _startGameEvents = startGameEvents;
        }

        public void Init()
        {
            _subscriptions.Add(_gameOverEvent.SubscribeTo(OnGameOver));
            _subscriptions.Add(_startGameEvents.SubscribeTo(OnStartGame));
        }
        
        private void OnStartGame(StartGameEvent _)
        {
            _gameState.ToggleGameState();
        }

        private void OnGameOver(GameOverEvent _)
        {
            _gameState.ToggleGameState();
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}