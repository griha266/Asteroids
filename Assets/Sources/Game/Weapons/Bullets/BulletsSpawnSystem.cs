using System;
using System.Collections.Generic;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.GameState;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Game.Weapons
{
    public class BulletsSpawnSystem : IInitializeable, IDisposable
    {
        private BulletFactory _laserFactory;
        private BulletFactory _regularBulletsFactory;
        private readonly DisposableCollection _subscriptions;
        private readonly EventBus<ShootEvent> _shootEvents;
        private readonly IReadOnlyList<BulletModel> _bullets;
        private readonly IModelsCollection _models;
        private readonly EventBus<DestroyModelEvent<BulletModel>> _bulletDestroyEvents;
        private readonly EventBus<GameOverEvent> _gameOverEvents;
        private readonly WeaponSettings _laserSettings;
        private readonly WeaponSettings _regularWeaponSettings;
        private readonly WeaponModel _regularWeapon;
        private readonly WeaponModel _laserWeapon;

        public BulletsSpawnSystem(
            EventBus<ShootEvent> shootEvents, 
            IReadOnlyList<BulletModel> bullets,
            IModelsCollection models,
            EventBus<DestroyModelEvent<BulletModel>> bulletDestroyEvents,
            EventBus<GameOverEvent> gameOverEvents, 
            WeaponModel laserWeapon, 
            WeaponModel regularWeapon, 
            WeaponSettings laserSettings, 
            WeaponSettings regularWeaponSettings
        ) {
            _subscriptions = new DisposableCollection();
            _shootEvents = shootEvents;
            _bullets = bullets;
            _models = models;
            _bulletDestroyEvents = bulletDestroyEvents;
            _gameOverEvents = gameOverEvents;
            _laserWeapon = laserWeapon;
            _regularWeapon = regularWeapon;
            _laserSettings = laserSettings;
            _regularWeaponSettings = regularWeaponSettings;
        }

        public void Init()
        {
            _laserFactory = new BulletFactory(
                _laserWeapon,
                _laserSettings.destroyBulletsOnCollision,
                _laserSettings.bulletPrefab,
                _models
            );
            _regularBulletsFactory = new BulletFactory(
                _regularWeapon,
                _regularWeaponSettings.destroyBulletsOnCollision,
                _regularWeaponSettings.bulletPrefab,
                _models
            );
            _laserFactory.Init();
            _regularBulletsFactory.Init();
            _subscriptions.Add(_shootEvents.SubscribeTo(OnShootEvent));
            _subscriptions.Add(_bulletDestroyEvents.SubscribeTo(OnBulletDestroyed));
            _subscriptions.Add(_gameOverEvents.SubscribeTo(OnGameOver));
        }
        
        private void OnGameOver(GameOverEvent _)
        {
            foreach (var bullet in _bullets)
            {
                DestroyBullet(bullet);
            }
        }

        private void OnBulletDestroyed(DestroyModelEvent<BulletModel> eventData)
        {
            DestroyBullet(eventData.Model);
        }

        private void DestroyBullet(BulletModel bullet)
        {
            var weapon = bullet.ParentWeapon;
            var factory = weapon.IsLaser ? _laserFactory : _regularBulletsFactory;
            factory.Destroy(bullet);
        }


        private void OnShootEvent(ShootEvent eventData)
        {
            var weapon = eventData.Weapon;
            var settings = weapon.IsLaser ? _laserSettings : _regularWeaponSettings;
            var factory = weapon.IsLaser ? _laserFactory : _regularBulletsFactory;
            var spawnParams = new KinematicBodyCreationParams(
                weapon.Transform.WorldPosition.Value,
                weapon.Transform.WorldRotation.Value,
                weapon.Transform.Up() * settings.spawnBulletVelocity
            );
            factory.Create(spawnParams);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

    }
    
    // public class WeaponShootingSystem : IInitializeable, IDisposable
    // {
    //     private readonly DisposableCollection _subscriptions;
    //     private readonly WeaponSettings _laserSettings;
    //     private readonly WeaponSettings _cannonSettings;
    //     private readonly AudioSettings _audioSettings;
    //     private readonly ModelsCollections _models;
    //     private readonly InputAction _laserShootAction;
    //     private readonly InputAction _cannonShootAction;
    //     private readonly EventBus<GameOverEvent> _gameOverEvents;
    //     private readonly EventBus<DestroyModelEvent<BulletModel>> _destroyBulletsEvents;
    //     private readonly EventBus<AudioEvent> _audioEvents;
    //     
    //     public WeaponShootingSystem(
    //         ProjectWideActions inputActions,
    //         ModelsCollections models,
    //         WeaponSettings laserSettings,
    //         WeaponSettings cannonSettings,
    //         EventBuses eventBuses, 
    //         AudioSettings audioSettings
    //     )
    //     {
    //         _subscriptions = new DisposableCollection();
    //         _models = models;
    //         _laserSettings = laserSettings;
    //         _cannonSettings = cannonSettings;
    //         _audioSettings = audioSettings;
    //         _laserShootAction = inputActions.Gameplay.FireLaser;
    //         _cannonShootAction = inputActions.Gameplay.FireCannon;
    //         _destroyBulletsEvents = eventBuses.GetBus<DestroyModelEvent<BulletModel>>();
    //         _gameOverEvents = eventBuses.GetBus<GameOverEvent>();
    //         _audioEvents = eventBuses.GetBus<AudioEvent>();
    //     }
    //
    //     public void Init()
    //     {
    //         var laserModel = _models.GetSharedOrThrow<LaserWeaponModel>();
    //         var cannonModel = _models.GetSharedOrThrow<RegularWeaponModel>();
    //         InitBullets(laserModel, _audioSettings.laserShootSound, _laserSettings, _laserShootAction);
    //         InitBullets(cannonModel, _audioSettings.regularShootSound,  _cannonSettings, _cannonShootAction);
    //     }
    //
    //     private void InitBullets(WeaponModel weaponModel, AudioClip shootAudio, WeaponSettings settings, InputAction fireAction)
    //     {
    //         var bulletsFactory = new BulletFactory(
    //             weaponModel,
    //             settings.destroyBulletsOnCollision,
    //             settings.bulletPrefab,
    //             _models
    //         );
    //         bulletsFactory.Init();
    //         _subscriptions.Add(bulletsFactory);
    //         var clearingSystem = new BulletClearingSystem(_models.GetModels<BulletModel>(), _gameOverEvents, _destroyBulletsEvents, bulletsFactory);
    //         _subscriptions.Add(clearingSystem);
    //         fireAction.performed += OnWeaponFire;
    //         _subscriptions.Add(new DisposeActionOnce(() => fireAction.performed -= OnWeaponFire));
    //         void OnWeaponFire(InputAction.CallbackContext context)
    //         {
    //             if (weaponModel.TryShoot())
    //             {
    //                 var spawnParams = new KinematicBodyCreationParams(
    //                     weaponModel.Transform.WorldPosition.Value,
    //                     weaponModel.Transform.WorldRotation.Value,
    //                     weaponModel.Transform.Up() * weaponModel.BulletsSpawnVelocity
    //                 );
    //                 bulletsFactory.Create(spawnParams);
    //                 _audioEvents.Push(new AudioEvent(shootAudio));
    //             }
    //         }
    //     }
    //     
    //     public void Dispose()
    //     {
    //         _subscriptions.Dispose();
    //     }
    // }
}