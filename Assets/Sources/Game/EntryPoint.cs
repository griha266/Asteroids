using Asteroids.Audio;
using Asteroids.Core.Models;
using Asteroids.Core.Systems;
using Asteroids.Game.Asteroids;
using Asteroids.Game.Enemies;
using Asteroids.Game.Explosion;
using Asteroids.Game.Audio;
using Asteroids.Game.GameState;
using Asteroids.Input;
using Asteroids.Game.Player;
using Asteroids.Game.Score;
using Asteroids.Game.UI;
using Asteroids.Game.Weapons;
using Asteroids.Physics;
using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private ApplicationUI applicationUI;
        [SerializeField] private SystemsRunner systemsRunner;
        [SerializeField] private AudioSource audioPlayer;
        
        private void Awake()
        {
#region Application-wide classes
            var sceneSize = new GameSceneSize(mainCamera, gameSettings.offscreenBorderSize);
            var inputActions = new ProjectWideActions();
            var eventBuses = new EventBus();
#endregion

#region Models
            var modelsCollection = new ModelsCollections();
            var scoreModel = new ScoreModel();
            var gameState = new GameStateModel();
            var playerModel = new PlayerModel(
                gameSettings.playerSettings.prefab.ColliderDescription, 
                gameSettings.playerSettings.maxVelocity
            );
            var laserWeapon = new WeaponModel(true, playerModel.Transform);
            var regularWeapon = new WeaponModel(false, playerModel.Transform);
#endregion

#region Systems
            var scoreInitializationSystem = new ScoreInitializationSystem(
                scoreModel, 
                gameState, 
                eventBuses.GetBus<AddScoreEvent>()
            );
            
            var collisionSystem = new CollisionDetectionSystem(
                eventBuses.GetBus<CollisionEvent>(), 
                modelsCollection.GetModels<IWithCollider>()
            );
            
            var kinematicSystem = new KinematicMovementSystem(
                sceneSize, 
                modelsCollection.GetModels<IWithVelocity>()
            );
            
            var dynamicObjectsCollisionSystem = new DynamicModelsCollisionSystem(
                eventBuses.GetBus<CollisionEvent>()
            );
            
            var weaponReloadSystem = new WeaponReloadSystem(
                gameState, 
                modelsCollection.GetModels<WeaponModel>()
            );
            
            var playerInitializationSystem = new PlayerInitializationSystem(
                modelsCollection,
                playerModel, 
                gameState, 
                gameSettings.playerSettings
            );
            
            var gameStateInitializationSystem = new GameStateInitialization(
                gameState, 
                eventBuses.GetBus<GameOverEvent>(),
                eventBuses.GetBus<StartGameEvent>()
            );
            
            var weaponsInitializationSystem = new WeaponsInitializationSystem(
                gameState,
                laserWeapon, 
                regularWeapon,
                modelsCollection,
                gameSettings.laserSettings,
                gameSettings.regularWeaponSettings
            );
            
            var bulletsSpawnSystem = new BulletsSpawnSystem(
                eventBuses.GetBus<ShootEvent>(),
                modelsCollection.GetModels<BulletModel>(),
                modelsCollection,
                eventBuses.GetBus<DestroyModelEvent<BulletModel>>(),
                eventBuses.GetBus<GameOverEvent>(),
                laserWeapon,
                regularWeapon,
                gameSettings.laserSettings,
                gameSettings.regularWeaponSettings
            );
            
            var bulletsDamageSystem = new BulletDamageSystem(
                eventBuses.GetBus<DestroyModelEvent<BulletModel>>(), 
                eventBuses.GetBus<CollisionEvent>()
            );
            
            var playerDragSystem = new PlayerDragSystem(playerModel, gameSettings.playerSettings.drag);
            
            var playerInputSystem = new PlayerInputSystem(
                inputActions.Gameplay.MoveForward,
                inputActions.Gameplay.RotateAxis,
                eventBuses.GetBus<AcceleratePlayerEvent>(),
                eventBuses.GetBus<RotatePlayerEvent>()
            );
            
            var playerMovementSystem = new PlayerMovementSystem(
                playerModel, 
                gameSettings.playerSettings.acceleration,
                gameSettings.playerSettings.rotationVelocity, 
                eventBuses.GetBus<AcceleratePlayerEvent>(), 
                eventBuses.GetBus<RotatePlayerEvent>()
            );
            
            var bulletsOutOfSceneSystem = new BulletsOutOfSceneDestructionSystem(
                modelsCollection.GetModels<BulletModel>(),
                sceneSize,
                eventBuses.GetBus<DestroyModelEvent<BulletModel>>()
            );
            
            var asteroidSpawnSystem = new AsteroidSpawnSystem(
                sceneSize,
                gameSettings.asteroidsSettings,
                modelsCollection.GetModels<AsteroidModel>(),
                modelsCollection,
                eventBuses.GetBus<DestroyModelEvent<AsteroidModel>>(),
                gameState
            );
            
            var asteroidDamageSystem = new AsteroidsDamageSystem(
                modelsCollection.GetModels<AsteroidModel>(), 
                eventBuses.GetBus<DestroyModelEvent<AsteroidModel>>()
            );
            
            var gameInputToggleSystem = new GameStateInputSystem(
                gameState, 
                inputActions, 
                eventBuses.GetBus<StartGameEvent>()
            );
            
            var uiInitializationSystem = new UIInitializationSystem(
                applicationUI,
                playerModel,
                laserWeapon,
                gameState,
                scoreModel
            );
            
            var explosionSystem = new ExplosionSystem(
                playerModel,
                gameSettings.explosionSettings.prefab,
                gameSettings.explosionSettings.duration,
                eventBuses.GetBus<DestroyModelEvent<AsteroidModel>>(),
                eventBuses.GetBus<DestroyModelEvent<EnemyModel>>(),
                eventBuses.GetBus<GameOverEvent>()
            );
            
            var enemySpawnSystem = new EnemySpawnSystem(
                gameState,
                sceneSize,
                modelsCollection.GetModels<EnemyModel>(),
                modelsCollection,
                gameSettings.enemySettings,
                eventBuses.GetBus<DestroyModelEvent<EnemyModel>>()
            );
            
            var enemyMovementSystem = new EnemyMovementSystem(
                playerModel,
                modelsCollection.GetModels<EnemyModel>(),
                modelsCollection.GetModels<AsteroidModel>(),
                gameSettings.enemySettings.maxVelocity,
                gameSettings.enemySettings.avoidDistance,
                gameSettings.enemySettings.maxAvoidanceForce
            );
            
            var enemyDamageSystem = new EnemyDamageSystem(
                modelsCollection.GetModels<EnemyModel>(),
                eventBuses.GetBus<DestroyModelEvent<EnemyModel>>()
            );
            
            var addingScoreSystem = new AddingScoreSystem(
                eventBuses.GetBus<AddScoreEvent>(),
                eventBuses.GetBus<DestroyModelEvent<AsteroidModel>>(),
                eventBuses.GetBus<DestroyModelEvent<EnemyModel>>()
            );
            
            var playerDamageSystem = new PlayerDamageSystem(
                eventBuses.GetBus<GameOverEvent>(),
                eventBuses.GetBus<CollisionEvent>()
            );
            
            var audioSystem = new AudioSystem(
                audioPlayer, 
                eventBuses.GetBus<PlayAudioEvent>()
            );
            
            var weaponInputSystems = new WeaponInputsSystem(
                laserWeapon,
                regularWeapon,
                inputActions.Gameplay.FireLaser,
                inputActions.Gameplay.FireCannon,
                eventBuses.GetBus<ShootEvent>()
            );

            var gameAudioSystem = new GameAudioSystem(
                gameSettings.audioSettings,
                eventBuses.GetBus<GameOverEvent>(),
                eventBuses.GetBus<DestroyModelEvent<AsteroidModel>>(),
                eventBuses.GetBus<DestroyModelEvent<EnemyModel>>(),
                eventBuses.GetBus<ShootEvent>(),
                eventBuses.GetBus<PlayAudioEvent>()
            );
#endregion

#region System registration

#region Initialization
            systemsRunner.AddInitializeable(scoreInitializationSystem);
            systemsRunner.AddInitializeable(dynamicObjectsCollisionSystem);
            systemsRunner.AddInitializeable(weaponReloadSystem);
            systemsRunner.AddInitializeable(playerInitializationSystem);
            systemsRunner.AddInitializeable(gameStateInitializationSystem);
            systemsRunner.AddInitializeable(asteroidSpawnSystem);
            systemsRunner.AddInitializeable(weaponsInitializationSystem);
            systemsRunner.AddInitializeable(bulletsSpawnSystem);
            systemsRunner.AddInitializeable(bulletsDamageSystem);
            systemsRunner.AddInitializeable(playerMovementSystem);
            systemsRunner.AddInitializeable(gameInputToggleSystem);
            systemsRunner.AddInitializeable(uiInitializationSystem);
            systemsRunner.AddInitializeable(explosionSystem);
            systemsRunner.AddInitializeable(enemySpawnSystem);
            systemsRunner.AddInitializeable(addingScoreSystem);
            systemsRunner.AddInitializeable(playerDamageSystem);
            systemsRunner.AddInitializeable(audioSystem);
            systemsRunner.AddInitializeable(weaponInputSystems);
            systemsRunner.AddInitializeable(gameAudioSystem);
#endregion

#region Regular update
            systemsRunner.AddRegularUpdateable(weaponReloadSystem);
            systemsRunner.AddRegularUpdateable(asteroidSpawnSystem);
            systemsRunner.AddRegularUpdateable(enemySpawnSystem);
            systemsRunner.AddRegularUpdateable(asteroidDamageSystem);
            systemsRunner.AddRegularUpdateable(enemyDamageSystem);
            systemsRunner.AddRegularUpdateable(explosionSystem);
#endregion
            // need to run at the end of every update
            systemsRunner.AddRegularUpdateable(modelsCollection);

#region Physics update
            systemsRunner.AddPhysicsUpdateable(bulletsOutOfSceneSystem);
            systemsRunner.AddPhysicsUpdateable(enemyMovementSystem);
            systemsRunner.AddPhysicsUpdateable(collisionSystem);
            systemsRunner.AddPhysicsUpdateable(kinematicSystem);
            systemsRunner.AddPhysicsUpdateable(playerDragSystem);
            systemsRunner.AddPhysicsUpdateable(playerInputSystem);
#endregion
            // need to run at the end of every update
            systemsRunner.AddPhysicsUpdateable(modelsCollection);

#region Dispose
            systemsRunner.AddDisposable(gameAudioSystem);
            systemsRunner.AddDisposable(weaponInputSystems);
            systemsRunner.AddDisposable(playerMovementSystem);
            systemsRunner.AddDisposable(bulletsSpawnSystem);
            systemsRunner.AddDisposable(gameStateInitializationSystem);
            systemsRunner.AddDisposable(weaponReloadSystem);
            systemsRunner.AddDisposable(audioSystem);
            systemsRunner.AddDisposable(playerDamageSystem);
            systemsRunner.AddDisposable(weaponsInitializationSystem);
            systemsRunner.AddDisposable(explosionSystem);
            systemsRunner.AddDisposable(addingScoreSystem);
            systemsRunner.AddDisposable(enemySpawnSystem);
            systemsRunner.AddDisposable(uiInitializationSystem);
            systemsRunner.AddDisposable(gameInputToggleSystem);
            systemsRunner.AddDisposable(asteroidSpawnSystem);
            systemsRunner.AddDisposable(bulletsDamageSystem);
            systemsRunner.AddDisposable(playerInitializationSystem);
            systemsRunner.AddDisposable(dynamicObjectsCollisionSystem);
            systemsRunner.AddDisposable(scoreInitializationSystem);
#endregion

#endregion
        }
    }
}