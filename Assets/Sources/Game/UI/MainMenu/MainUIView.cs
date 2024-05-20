using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Game.UI
{
    public class MainUIView : UIView
    {
        [SerializeField] private Text scoreText;

        public void SetMaxScore(int score)
        {
            scoreText.text = $"Your best score is {score}";
        }
    }
}