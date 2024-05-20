using UnityEngine;

namespace Asteroids.Physics
{
    public class ColliderDescriptionComponent : MonoBehaviour
    {
        public ColliderDescription ColliderDescription;
        
        private void OnDrawGizmosSelected()
        {
            DrawColliderGizmo(Color.cyan);
        }

        private void OnDrawGizmos()
        {
            DrawColliderGizmo(Color.green);
        }

        private void DrawColliderGizmo(Color color)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, ColliderDescription.ColliderRadius);
            Gizmos.color = oldColor;
        }
    }
}