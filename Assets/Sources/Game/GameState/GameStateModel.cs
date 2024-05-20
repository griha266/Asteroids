using Asteroids.Utils;

namespace Asteroids.Game.GameState
{
    public class GameStateModel
    {
        private readonly ReactiveProperty<bool> _isGameLoop = new();
        public IReadOnlyReactiveProperty<bool> IsGameLoop => _isGameLoop;


        public void ToggleGameState()
        {
            _isGameLoop.Value = !_isGameLoop.Value;
        }
    }
}