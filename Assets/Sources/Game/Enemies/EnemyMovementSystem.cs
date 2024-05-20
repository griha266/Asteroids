using System.Collections.Generic;
using System.Linq;
using Asteroids.Core.Systems;
using Asteroids.Game.Asteroids;
using Asteroids.Game.Player;
using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Enemies
{
    public class EnemyMovementSystem : IUpdateable
    {
        private readonly PlayerModel _player;
        private readonly IReadOnlyList<EnemyModel> _enemies;
        private readonly IReadOnlyList<AsteroidModel> _asteroids;
        private readonly float _maxSpeed;
        private readonly float _avoidDistance;
        private readonly float _maxAvoidanceForce;

        public EnemyMovementSystem(
            PlayerModel player,
            IReadOnlyList<EnemyModel> enemies,
            IReadOnlyList<AsteroidModel> asteroids,
            float maxSpeed,
            float avoidDistance,
            float maxAvoidanceForce
        )
        {
            _player = player;
            _enemies = enemies;
            _asteroids = asteroids;
            _maxSpeed = maxSpeed;
            _avoidDistance = avoidDistance;
            _maxAvoidanceForce = maxAvoidanceForce;
        }

        public void Update(float deltaTime)
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.IsEnabled.Value)
                {
                    continue;
                }

                var enemyPosition = enemy.Transform.WorldPosition.Value;

                var resultVelocity = Vector2.zero;
                if (_player.IsEnabled.Value)
                {
                    // go to player and avoid collision
                    var toPlayerVelocity = GetVelocityTowardsPlayer(
                        enemyPosition,
                        _player.Transform.WorldPosition.Value,
                        _maxSpeed
                    );
                    resultVelocity += toPlayerVelocity;
                }

                var avoidVelocity = GetAvoidObstacleVelocity(
                    enemyPosition,
                    _asteroids.Concat<IWithCollider>(_enemies.Where(e => e != enemy)),
                    _avoidDistance,
                    _maxAvoidanceForce
                );
                resultVelocity += avoidVelocity;

                enemy.Velocity.Value = resultVelocity;
                var desiredUp = enemy.Velocity.Value.normalized;
                var angle = Vector2.SignedAngle(Vector2.up, desiredUp);
                enemy.Transform.WorldRotation.Value = angle;
            }
        }

        private static Vector2 GetVelocityTowardsPlayer(Vector2 enemyPosition, Vector2 playerPosition, float maxVelocity)
        {
            var desiredVelocity = (playerPosition - enemyPosition).normalized * maxVelocity;
            return desiredVelocity;
        }

        private static Vector2 GetAvoidObstacleVelocity(
            Vector2 enemyPosition, 
            IEnumerable<IWithCollider> obstacles, 
            float avoidDistance, 
            float maxAvoidForce
        ) {
            var avoidance = Vector2.zero;

            foreach (var obstacle in obstacles)
            {
                var obstaclePosition = obstacle.Transform.WorldPosition.Value;
                var difference = obstaclePosition - enemyPosition;
                var distance = difference.magnitude;
        
                if (distance < avoidDistance + obstacle.Collider.Radius)
                {
                    var awayFromAsteroid = (enemyPosition - obstaclePosition).normalized;
                    var strength = Mathf.Clamp01((avoidDistance + obstacle.Collider.Radius - distance) / (avoidDistance + obstacle.Collider.Radius));
                    avoidance += awayFromAsteroid * (strength * maxAvoidForce);
                }
            }

            return avoidance;
        }

    }
}