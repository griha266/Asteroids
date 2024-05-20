using System.Collections.Generic;
using Asteroids.Core.Systems;
using Asteroids.Transform;

namespace Asteroids.Physics
{
    public class KinematicMovementSystem : IUpdateable
    {
        private readonly IReadOnlyList<IWithVelocity> _models;
        private readonly GameSceneSize _sceneSize;

        public KinematicMovementSystem(GameSceneSize sceneSize, IReadOnlyList<IWithVelocity> models)
        {
            _models = models;
            _sceneSize = sceneSize;
        }

        public void Update(float deltaTime)
        {
            foreach (var model in _models)
            {
                if (!model.IsEnabled.Value)
                {
                    continue;
                }
                
                var maxVelocity = model.MaxVelocity;
                if (model.Velocity.Value.sqrMagnitude > maxVelocity * maxVelocity)
                {
                    model.Velocity.Value = model.Velocity.Value.normalized * maxVelocity;
                }
                
                var movementOffset = model.Velocity.Value * deltaTime;
                var previousPosition = model.Transform.WorldPosition.Value;
                var newPosition = previousPosition + movementOffset;

                // // Reappearing on the other side of the screen
                if (previousPosition.x > _sceneSize.TopRight.x)
                {
                    newPosition.x += _sceneSize.BottomLeft.x - _sceneSize.TopRight.x;
                } else if (previousPosition.x < _sceneSize.BottomLeft.x)
                {
                    newPosition.x += _sceneSize.TopRight.x - _sceneSize.BottomLeft.x;
                }
                if (previousPosition.y > _sceneSize.TopRight.y)
                {
                    newPosition.y += _sceneSize.BottomLeft.y - _sceneSize.TopRight.y;
                } else if (previousPosition.y < _sceneSize.BottomLeft.y)
                {
                    newPosition.y += _sceneSize.TopRight.y - _sceneSize.BottomLeft.y;
                }
                
                model.Transform.WorldPosition.Value = newPosition;
                
            }
        }
    }

}