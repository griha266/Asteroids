using UnityEngine;

namespace Asteroids.Utils
{
    public static class MathUtils
    {
        public static Vector2 RotatePoint(Vector2 point, float degrees)
        {
            var radians = Mathf.PI * degrees / 180.0f;
            var cos = Mathf.Cos(radians);
            var sin = Mathf.Sin(radians);

            return new Vector2(
                cos * point.x - sin * point.y,
                sin * point.x + cos * point.y
            );
        }

        public static Vector2 XY(this Vector3 v) => new(v.x, v.y);
    }
    
}