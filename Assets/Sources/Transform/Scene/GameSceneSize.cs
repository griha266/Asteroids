using UnityEngine;

namespace Asteroids.Transform
{
    public readonly struct GameSceneSize
    {
        public readonly Vector2 TopRight;
        public readonly Vector2 BottomLeft;
        public readonly Vector2 Center;

        public GameSceneSize(Camera camera, float offscreenBorderSize)
        {
            var cameraZOffset = camera.transform.position.z;
            var topRightPoint = camera.ViewportToWorldPoint(new Vector3(1, 1, -cameraZOffset));
            var bottomLeftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, -cameraZOffset));
            topRightPoint += new Vector3(offscreenBorderSize, offscreenBorderSize, 0);
            bottomLeftPoint -= new Vector3(offscreenBorderSize, offscreenBorderSize, 0);
            TopRight = topRightPoint;
            BottomLeft = bottomLeftPoint;
            Center = (TopRight + BottomLeft) * 0.5f;
        }

        public override string ToString()
        {
            return $"{nameof(TopRight)}:{TopRight}, {nameof(BottomLeft)}:{BottomLeft},{nameof(Center)}:{Center}";
        }

        public bool IsInside(Vector2 point)
        {
            var insideX = point.x < TopRight.x && point.x > BottomLeft.x;
            var insideY = point.y < TopRight.y && point.y > BottomLeft.y;
            return insideX && insideY;
        }
    }
}