using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Game.UI
{
    public class GameStateUIView : UIView
    {
        [SerializeField] private TextViewReference currentPositionText;
        [SerializeField] private TextViewReference currentRotationText;
        [SerializeField] private TextViewReference currentVelocityText;
        [SerializeField] private TextViewReference currentScoreText;
        [SerializeField] private TextViewReference laserAmmoText;
        [SerializeField] private TextViewReference laserChargeText;
        [SerializeField] private Image laserChargeIndicator;
        
        public void SetCurrentPositionText(Vector2 position) 
        { 
            currentPositionText.TextComponent.text = string.Format(currentPositionText.TextTemplate, position.x, position.y);
        }
        
        public void SetCurrentRotationText(float rotation) 
        { 
            currentRotationText.TextComponent.text = string.Format(currentRotationText.TextTemplate, rotation);
        }
        
        public void SetCurrentVelocityText(Vector2 velocity)
        {
            currentVelocityText.TextComponent.text = string.Format(currentVelocityText.TextTemplate, velocity.x, velocity.y);
        }
        
        public void SetCurrentScoreText(int score)
        {
            currentScoreText.TextComponent.text = string.Format(currentScoreText.TextTemplate, score);
        }

        public void SetLaserAmmoText(int laserAmmo, int maxAmmo) 
        { 
            laserAmmoText.TextComponent.text = string.Format(laserAmmoText.TextTemplate, laserAmmo, maxAmmo);
        }
        
        public void SetLaserCharge(float laserCharge)
        {
            laserChargeIndicator.fillAmount = laserCharge;
            laserChargeText.TextComponent.text = string.Format(laserChargeText.TextTemplate, laserCharge * 100);
        }
        
    }
}