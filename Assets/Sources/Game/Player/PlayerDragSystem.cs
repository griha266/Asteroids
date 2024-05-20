using Asteroids.Core.Systems;
using UnityEngine;

namespace Asteroids.Game.Player
{
    public class PlayerDragSystem : IUpdateable
    {
        private readonly PlayerModel _player;
        private readonly float _dragForce;

        public PlayerDragSystem(PlayerModel player, float dragForce)
        {
            _player = player;
            _dragForce = dragForce;
        }

        public void Update(float deltaTime)
        {
            var currentVelocity = _player.Velocity.Value;
            var inverted = currentVelocity * -1;
            var velocitySubtraction = inverted.normalized * (_dragForce * deltaTime);
            var resultVelocity = currentVelocity + velocitySubtraction;
            if (Mathf.Abs(velocitySubtraction.x) > Mathf.Abs(currentVelocity.x))
            {
                resultVelocity.x = 0;
            }
            if (Mathf.Abs(velocitySubtraction.y) > Mathf.Abs(currentVelocity.y))
            {
                resultVelocity.y = 0;
            }
            _player.Velocity.Value = resultVelocity;
        }

    }
}