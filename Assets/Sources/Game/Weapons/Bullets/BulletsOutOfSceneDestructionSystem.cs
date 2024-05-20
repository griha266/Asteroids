using System.Collections.Generic;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Transform;
using Asteroids.Utils;

namespace Asteroids.Game.Weapons
{
    public class BulletsOutOfSceneDestructionSystem : IUpdateable
    {
        private readonly EventBus<DestroyModelEvent<BulletModel>> _destroyBulletEvents;
        private readonly GameSceneSize _sceneSize;
        private readonly IReadOnlyList<BulletModel> _bullets;

        public BulletsOutOfSceneDestructionSystem(
            IReadOnlyList<BulletModel> bullets,
            GameSceneSize sceneSize, 
            EventBus<DestroyModelEvent<BulletModel>> destroyBulletEvents
        )
        {
            _bullets = bullets;
            _sceneSize = sceneSize;
            _destroyBulletEvents = destroyBulletEvents;
        }
        
        public void Update(float deltaTime)
        {
            foreach (var bullet in _bullets)
            {
                if (!bullet.IsEnabled.Value)
                {
                    continue;
                }
                
                var worldPosition = bullet.Transform.WorldPosition.Value;
                if (!_sceneSize.IsInside(worldPosition)) {
                    _destroyBulletEvents.Push(new DestroyModelEvent<BulletModel>(bullet));
                }
            }
        }
    }
}