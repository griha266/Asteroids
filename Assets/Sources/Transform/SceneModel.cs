using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Transform
{
    public abstract class SceneModel : IWithTransform 
    {
        public GameTransform Transform { get; }
        public IReactiveProperty<bool> IsEnabled { get; }

        protected SceneModel(Vector2 initialPosition, float initialRotation, GameTransform parent = null)
        {
            IsEnabled = new ReactiveProperty<bool>();
            Transform = new GameTransform(initialPosition, initialRotation, parent);
        }
    }
}