namespace Asteroids.Game.Player
{
    public readonly struct AcceleratePlayerEvent
    {
        public readonly float DeltaTime;
        public readonly float Value;

        public AcceleratePlayerEvent(float value, float deltaTime)
        {
            Value = value;
            DeltaTime = deltaTime;
        }
    }
}