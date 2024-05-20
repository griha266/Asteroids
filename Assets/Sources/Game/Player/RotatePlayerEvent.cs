namespace Asteroids.Game.Player
{
    public readonly struct RotatePlayerEvent
    {
        public readonly float DeltaTime;
        public readonly float Value;

        public RotatePlayerEvent(float value, float deltaTime)
        {
            Value = value;
            DeltaTime = deltaTime;
        }
    }
}