using Asteroids.Core.Models;
using Asteroids.Physics;
using Asteroids.Transform;
using UnityEngine;

namespace Asteroids.Game.Enemies
{
    public class EnemyFactory : SceneFactoryBase<EnemyModel, KinematicBodyCreationParams>
    {
        private readonly EnemySettings _settings;
        public EnemyFactory(EnemySettings settings, IModelsCollection models, int preloadedModelsCount = 0) 
            : base(settings.enemyPrefab.gameObject, models, preloadedModelsCount)
        {
            _settings = settings;
        }

        protected override EnemyModel BuildModelInstance()
        {
            return new EnemyModel(
                _settings.enemyPrefab.ColliderDescription,
                true,
                _settings.maxVelocity,
                Vector2.zero, 
                Vector2.zero, 
                0
            );
        }

        protected override void OnModelGet(EnemyModel model, KinematicBodyCreationParams creationParams)
        {
            model.IsEnabled.Value = true;
            model.Health = 1;
            model.Transform.WorldPosition.Value = creationParams.Position;
            model.Transform.WorldRotation.Value = creationParams.Rotation;
        }

        protected override void OnModelRelease(EnemyModel model)
        {
            model.IsEnabled.Value = false;
        }
    }
}