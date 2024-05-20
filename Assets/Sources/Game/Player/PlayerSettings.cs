using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Player
{
    [CreateAssetMenu(menuName = "Game/Create Player settings", fileName = "PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        public ColliderDescriptionComponent prefab;
        public float maxVelocity;
        public float acceleration;
        public float rotationVelocity;
        public float drag;
    }
}