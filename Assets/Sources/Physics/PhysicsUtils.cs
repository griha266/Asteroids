using Asteroids.Transform;
using UnityEngine;

namespace Asteroids.Physics
{
    public static class PhysicsUtils
    {
        public static bool IsInsideMask(this LayerMask layerMask, LayerMask comparedMask)
        {
            var result = (layerMask & comparedMask) == layerMask;
            return result;
        }
        
        public static KinematicBodyCreationParams CreateRandomSpawnParams(
            GameSceneSize sceneSize, 
            float minSpawnVelocity, 
            float maxSpawnVelocity
        ) {
            var spawnPosition = GameTransformUtils.GetRandomSceneBorderPosition(sceneSize);
            var velocityDirection = (sceneSize.Center - spawnPosition).normalized;
            var spawnVelocityMagnitude = Random.Range(minSpawnVelocity, maxSpawnVelocity);
            var spawnVelocity = velocityDirection * spawnVelocityMagnitude;
            var rotation = Vector2.Angle(Vector2.up, velocityDirection);
            return new KinematicBodyCreationParams(
                spawnPosition,
                rotation,
                spawnVelocity
            );
        }
    }
}