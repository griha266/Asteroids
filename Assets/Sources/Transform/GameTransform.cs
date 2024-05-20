using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Transform
{
    public class GameTransform
    {
        public IReactiveProperty<GameTransform> Parent { get; }
        public IReactiveProperty<Vector2> LocalPosition { get; }
        public IReactiveProperty<float> LocalRotationDegrees { get; }
        public IReactiveProperty<Vector2> WorldPosition { get; }
        public IReactiveProperty<float> WorldRotation { get; }
        
        public GameTransform(Vector2 position, float rotationDegrees, GameTransform parent = null)
        {
            LocalPosition = new ReactiveProperty<Vector2>(position);
            // Clamp to one circle
            LocalRotationDegrees = new ReactiveProperty<float>(rotationDegrees)
                .MapTwoWay(
                    getter: rotation => rotation,
                    setter: rotation =>
                    {
                        if (rotation >= 360)
                        {
                            var fullCircleCount = (int)rotation / 360;
                            rotation -= fullCircleCount * 360;
                        }

                        return rotation;
                    }
                );
            
            Parent = new ReactiveProperty<GameTransform>(parent);
            
            WorldPosition = LocalPosition.MapTwoWay(
                localPosition => Parent.Value?.TransformPoint(localPosition) ?? localPosition,
                worldPosition => Parent.Value?.InverseTransformPoint(worldPosition) ?? worldPosition
            );
            
            WorldRotation = LocalRotationDegrees.MapTwoWay(
                localRotation => Parent.Value?.TransformRotation(localRotation) ?? localRotation,
                worldRotation => Parent.Value?.InverseTransformRotation(worldRotation) ?? worldRotation
            );
        }

        public float TransformRotation(float localDegrees)
        {
            if (Parent.Value != null)
            {
                localDegrees = Parent.Value.TransformRotation(localDegrees);
            }

            localDegrees += LocalRotationDegrees.Value;
            return localDegrees;
        }

        public float InverseTransformRotation(float worldRotation)
        {
            if (Parent.Value != null)
            {
                worldRotation = Parent.Value.InverseTransformRotation(worldRotation);
            }

            worldRotation -= LocalRotationDegrees.Value;
            return worldRotation;
        }
        
        public Vector2 TransformPoint(Vector2 localPoint)
        {
            var worldPoint = MathUtils.RotatePoint(localPoint, LocalRotationDegrees.Value);
            worldPoint += LocalPosition.Value;

            if (Parent.Value != null)
            {
                worldPoint = Parent.Value.TransformPoint(worldPoint);
            }

            return worldPoint;
        }

        public Vector2 Up() => MathUtils.RotatePoint(Vector2.up, WorldRotation.Value);

        public Vector2 InverseTransformPoint(Vector2 worldPoint)
        {
            if (Parent.Value != null)
            {
                worldPoint = Parent.Value.InverseTransformPoint(worldPoint);
            }

            worldPoint -= LocalPosition.Value;
            worldPoint = MathUtils.RotatePoint(worldPoint, -LocalRotationDegrees.Value);

            return worldPoint; 
        }

    }
}