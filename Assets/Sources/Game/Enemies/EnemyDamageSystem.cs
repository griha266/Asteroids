using System.Collections.Generic;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Utils;

namespace Asteroids.Game.Enemies
{
    public class EnemyDamageSystem : IUpdateable
    {
        private readonly IReadOnlyList<EnemyModel> _enemies;
        private readonly EventBus<DestroyModelEvent<EnemyModel>> _destroyEnemyEvents;

        public EnemyDamageSystem(IReadOnlyList<EnemyModel> enemies, EventBus<DestroyModelEvent<EnemyModel>> destroyEnemyEvents)
        {
            _enemies = enemies;
            _destroyEnemyEvents = destroyEnemyEvents;
        }
        
        public void Update(float deltaTime)
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.IsEnabled.Value)
                {
                    continue;
                }

                if (enemy.Health < 1)
                {
                    _destroyEnemyEvents.Push(new DestroyModelEvent<EnemyModel>(enemy));
                }
            }
        }
    }
}