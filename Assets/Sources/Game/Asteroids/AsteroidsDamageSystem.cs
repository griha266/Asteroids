using System.Collections.Generic;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Utils;

namespace Asteroids.Game.Asteroids
{
    public class AsteroidsDamageSystem : IUpdateable
    {
        private readonly IReadOnlyList<AsteroidModel> _asteroids;
        private readonly EventBus<DestroyModelEvent<AsteroidModel>> _destroyEvents;
        public AsteroidsDamageSystem(IReadOnlyList<AsteroidModel> asteroids, EventBus<DestroyModelEvent<AsteroidModel>> destroyEvents)
        {
            _asteroids = asteroids;
            _destroyEvents = destroyEvents;
        }

        public void Update(float deltaTime)
        {
            foreach (var model in _asteroids)
            {
                bool shouldDestroy = false;
                if (model.IsBig)
                {
                    if (model.Health < 2)
                    {
                        shouldDestroy = true;
                    }
                }
                else
                {
                    if (model.Health < 1)
                    {
                        shouldDestroy = true;
                    }
                }
                
                if (shouldDestroy)
                {
                    _destroyEvents.Push(new DestroyModelEvent<AsteroidModel>(model));
                }
            }
        }
    }
}