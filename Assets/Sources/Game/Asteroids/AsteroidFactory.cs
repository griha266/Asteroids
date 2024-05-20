using Asteroids.Core.Models;
using Asteroids.Physics;
using Asteroids.Transform;
using UnityEngine;

namespace Asteroids.Game.Asteroids
{
    public class AsteroidFactory : SceneFactoryBase<AsteroidModel, KinematicBodyCreationParams>
    {
        private readonly float _maxAvailableVelocity;
        private readonly ColliderDescriptionComponent _prefab;
        private readonly bool _isBig;

        public AsteroidFactory(bool isBig, float maxAvailableVelocity, IModelsCollection models, ColliderDescriptionComponent prefab) 
            : base(prefab.gameObject, models, 3)
        {
            _maxAvailableVelocity = maxAvailableVelocity;
            _isBig = isBig;
            _prefab = prefab;
        }


        protected override AsteroidModel BuildModelInstance()
        {
            return new AsteroidModel(
                _isBig,
                _prefab.ColliderDescription,
                true,
                _maxAvailableVelocity,
                Vector2.zero, 
                Vector2.zero, 
                0
            );
        }

        protected override void OnModelGet(AsteroidModel model, KinematicBodyCreationParams creationParams)
        {
            model.Transform.WorldPosition.Value = creationParams.Position;
            model.Transform.WorldRotation.Value = creationParams.Rotation;
            model.Velocity.Value = creationParams.Velocity;
            model.Health = _isBig ? 2 : 1;
            model.IsEnabled.Value = true;
        }

        protected override void OnModelRelease(AsteroidModel model)
        {
            model.IsEnabled.Value = false;
        }
    }
}