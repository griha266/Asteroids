using System;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.Damage;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Game.Weapons
{
    public class BulletDamageSystem : IInitializeable, IDisposable
    {
        private readonly DisposableCollection _subscriptions;
        private readonly EventBus<DestroyModelEvent<BulletModel>> _destroyBulletEvents;
        private readonly EventBus<CollisionEvent> _collisionEvents;

        public BulletDamageSystem(
            EventBus<DestroyModelEvent<BulletModel>> destroyBulletEvents, 
            EventBus<CollisionEvent> collisionEvents
        ) {
            _subscriptions = new DisposableCollection();
            _destroyBulletEvents = destroyBulletEvents;
            _collisionEvents = collisionEvents;
        }

        public void Init()
        {
            _subscriptions.Add(_collisionEvents.SubscribeTo(OnCollisionEvent));
        }
        
        private void OnCollisionEvent(CollisionEvent obj)
        {
            if (obj.Collision.FirstCollider.Parent is BulletModel bulletModel)
            {
                if (obj.Collision.SecondCollider.Parent is IDamageable damageable)
                {
                    damageable.Health -= bulletModel.ParentWeapon.IsLaser ? 2 : 1;
                }

                if (bulletModel.DestroyOnCollision)
                {
                    _destroyBulletEvents.Push(new DestroyModelEvent<BulletModel>(bulletModel));
                }
            } 
            else if (obj.Collision.SecondCollider.Parent is BulletModel otherBulletModel)
            {
                if (obj.Collision.FirstCollider.Parent is IDamageable damageable)
                {
                    damageable.Health -= otherBulletModel.ParentWeapon.IsLaser ? 2 : 1;
                    
                }

                if (otherBulletModel.DestroyOnCollision)
                {
                    _destroyBulletEvents.Push(new DestroyModelEvent<BulletModel>(otherBulletModel));
                }
            }
        }

        
        public void Dispose()
        {
            _subscriptions.Dispose();
        }

    }
}