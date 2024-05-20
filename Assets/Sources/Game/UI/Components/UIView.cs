using UnityEngine;

namespace Asteroids.Game.UI
{
    public abstract class UIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvas;

        public void SetVisibility(bool isVisible)
        {
            canvas.alpha = isVisible ? 1 : 0;
            canvas.interactable = isVisible;
            canvas.blocksRaycasts = isVisible;
        }
    }
}