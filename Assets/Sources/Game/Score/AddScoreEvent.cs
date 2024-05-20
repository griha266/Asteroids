namespace Asteroids.Game.Score
{
    public readonly struct AddScoreEvent
    {
        public readonly int AddScoreCount;

        public AddScoreEvent(int addScoreCount)
        {
            AddScoreCount = addScoreCount;
        }
    }
}