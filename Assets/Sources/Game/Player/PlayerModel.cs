using Asteroids.Physics;
using UnityEngine;

namespace Asteroids.Game.Player
{
    public class PlayerModel : DynamicModel  
    {
        public PlayerModel(ColliderDescription colliderDescription, float maxVelocity) 
            : base(
                false, 
                colliderDescription,
                true, 
                maxVelocity,
                Vector2.zero, 
                Vector2.zero, 
                0
        ) {
        }

    }
}