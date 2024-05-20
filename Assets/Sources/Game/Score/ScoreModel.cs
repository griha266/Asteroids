using Asteroids.Utils;

namespace Asteroids.Game.Score
{
    public class ScoreModel
    {
        public ReactiveProperty<int> MaxScore { get; } = new();
        public ReactiveProperty<int> CurrentScore { get; } = new();

        public void ResetCurrentScore()
        {
            CurrentScore.Value = 0;
        }
    }
}