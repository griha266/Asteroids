using Asteroids.Core.Models;
using Asteroids.Physics;
using Asteroids.Transform;

namespace Asteroids.Game.Weapons
{
    public class BulletFactory : SceneFactoryBase<BulletModel, KinematicBodyCreationParams>
    {
        private readonly WeaponModel _weapon;
        private readonly bool _bulletsDestroyOnCollision;
        private readonly ColliderDescriptionComponent _prefab;


        public BulletFactory(
            WeaponModel weapon,
            bool bulletsDestroyOnCollision,
            ColliderDescriptionComponent prefab, 
            IModelsCollection models
        ) : base(prefab.gameObject, models, 10)
        {
            _weapon = weapon;
            _prefab = prefab;
            _bulletsDestroyOnCollision = bulletsDestroyOnCollision;
        }

        protected override BulletModel BuildModelInstance()
        {
            var model = new BulletModel(
                _weapon,
                _bulletsDestroyOnCollision,
                _prefab.ColliderDescription,
                false
            );
            return model;
        }

        protected override void OnModelGet(BulletModel model, KinematicBodyCreationParams creationParams)
        {
            model.Transform.WorldPosition.Value = creationParams.Position;
            model.Transform.WorldRotation.Value = creationParams.Rotation;
            model.Velocity.Value = creationParams.Velocity;
            model.IsEnabled.Value = true;
            model.Collider.IsEnabled.Value = true;
        }

        protected override void OnModelRelease(BulletModel model)
        {
            model.IsEnabled.Value = false;
        }
    }
}