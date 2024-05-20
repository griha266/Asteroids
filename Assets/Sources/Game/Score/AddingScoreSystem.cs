using System;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.Asteroids;
using Asteroids.Game.Enemies;
using Asteroids.Utils;

namespace Asteroids.Game.Score
{
    public class AddingScoreSystem : IInitializeable, IDisposable
    {
        private readonly EventBus<AddScoreEvent> _addingScoreEvents;
        private readonly EventBus<DestroyModelEvent<AsteroidModel>> _asteroidsDestroyEvents;
        private readonly EventBus<DestroyModelEvent<EnemyModel>> _enemiesDestroyEvents;
        private readonly DisposableCollection _subscriptions;

        public AddingScoreSystem(
            EventBus<AddScoreEvent> addingScoreEvents,
            EventBus<DestroyModelEvent<AsteroidModel>> asteroidsDestroyEvents,
            EventBus<DestroyModelEvent<EnemyModel>> enemiesDestroyEvents
        )
        {
            _addingScoreEvents = addingScoreEvents;
            _asteroidsDestroyEvents = asteroidsDestroyEvents;
            _enemiesDestroyEvents = enemiesDestroyEvents;
            _subscriptions = new DisposableCollection();
        }

        public void Init()
        {
            _subscriptions.Add(_asteroidsDestroyEvents.SubscribeTo(_ => OnEnvironmentModelDestroyed()));
            _subscriptions.Add(_enemiesDestroyEvents.SubscribeTo(_ => OnEnvironmentModelDestroyed()));
        }

        private void OnEnvironmentModelDestroyed()
        {
            _addingScoreEvents.Push(new AddScoreEvent(1));
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}