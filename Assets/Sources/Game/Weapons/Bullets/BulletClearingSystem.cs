namespace Asteroids.Core
{
    // public class BulletClearingSystem : IDisposable
    // {
    //     private readonly IReadOnlyList<BulletModel> _bullets;
    //     private readonly DisposableCollection _subscriptions;
    //     private readonly BulletFactory _bulletFactory;
    //
    //     public BulletClearingSystem(IReadOnlyList<BulletModel> bullets, EventBus<GameOverEvent> gameOverEvent, EventBus<DestroyModelEvent<BulletModel>> destroyBullets, BulletFactory bulletFactory)
    //     {
    //         _subscriptions = new DisposableCollection();
    //         _bullets = bullets;
    //         _bulletFactory = bulletFactory;
    //         _subscriptions.Add(destroyBullets.SubscribeTo(OnDestroyBulletEvent));
    //         _subscriptions.Add(gameOverEvent.SubscribeTo(OnGameOver));
    //     }
    //
    //     private void OnGameOver(GameOverEvent obj)
    //     {
    //         foreach (var bullet in _bullets)
    //         {
    //             if (_bulletFactory.Weapon == bullet.ParentWeapon)
    //             {
    //                 _bulletFactory.Destroy(bullet);
    //             }
    //         }
    //     }
    //
    //     private void OnDestroyBulletEvent(DestroyModelEvent<BulletModel> eventData)
    //     {
    //         if (_bulletFactory.Weapon == eventData.Model.ParentWeapon)
    //         {
    //             _bulletFactory.Destroy(eventData.Model);
    //         }
    //     }
    //
    //     public void Dispose()
    //     {
    //         _subscriptions.Dispose();
    //     }
    // }
}