using System;
using Asteroids.Core.Systems;
using Asteroids.Input;
using Asteroids.Utils;
using UnityEngine.InputSystem;

namespace Asteroids.Game.GameState
{
    public class GameStateInputSystem : IInitializeable, IDisposable
    {
        private readonly GameStateModel _gameState;
        private readonly ProjectWideActions _inputActions;
        private readonly DisposableCollection _subscriptions;
        private readonly EventBus<StartGameEvent> _startGameEvents;

        public GameStateInputSystem(GameStateModel gameState, ProjectWideActions inputActions, EventBus<StartGameEvent> startGameEvents)
        {
            _subscriptions = new DisposableCollection();
            _gameState = gameState;
            _inputActions = inputActions;
            _startGameEvents = startGameEvents;
        }

        public void Init()
        {
            _subscriptions.Add(_gameState.IsGameLoop.SubscribeAndRefresh(OnGameStateChanged));
            _inputActions.MainScreen.StartGame.performed += OnStartGame;
        }

        private void OnStartGame(InputAction.CallbackContext obj)
        {
            _startGameEvents.Push(new StartGameEvent());
        }

        private void OnGameStateChanged(bool isGameLoop)
        {
            if (isGameLoop)
            {
                _inputActions.MainScreen.Disable();
                _inputActions.Gameplay.Enable();
            }
            else
            {
                _inputActions.MainScreen.Enable();
                _inputActions.Gameplay.Disable();
                
            }
        }

        public void Dispose()
        {
            _inputActions.MainScreen.StartGame.performed -= OnStartGame;
            _subscriptions.Dispose();
        }
    }
}