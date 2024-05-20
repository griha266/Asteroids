using UnityEngine;

namespace Asteroids.Transform
{
    public static class GameTransformUtils
    {
        public static Vector2 GetRandomSceneBorderPosition(GameSceneSize sceneSize)
        {
            var result = new Vector2();
            var isHorizontal = Random.Range(0, 1f) > 0.5f;
            var isFirstSide = Random.Range(0, 1f) > 0.5f;
            if (isHorizontal)
            {
                result.x = isFirstSide ? sceneSize.TopRight.x : sceneSize.BottomLeft.x;
                result.y = Random.Range(sceneSize.BottomLeft.y, sceneSize.TopRight.y);
            }
            else
            {
                result.y = isFirstSide ? sceneSize.TopRight.y : sceneSize.BottomLeft.y;
                result.x = Random.Range(sceneSize.BottomLeft.x, sceneSize.TopRight.x);
            }

            return result;
        }
    }
}