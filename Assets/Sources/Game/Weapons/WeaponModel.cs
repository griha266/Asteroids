using Asteroids.Transform;
using Asteroids.Utils;
using UnityEngine;

namespace Asteroids.Game.Weapons
{
    public class WeaponModel : SceneModel
    {
        private readonly ReactiveProperty<int> _currentAmmo;
        public bool IsLaser { get; }

        public ReactiveProperty<float> CurrentReloadTimer { get; }
        public IReadOnlyReactiveProperty<int> CurrentAmmo => _currentAmmo;
        public float ReloadDuration { get; private set; }
        public int MaxAmmo { get; private set; }
        public bool FullMagazine => _currentAmmo.Value == MaxAmmo;

        public WeaponModel(
            bool isLaser,
            GameTransform parent
        ) : base(Vector2.zero, 0, parent)
        {
            IsLaser = isLaser;
            _currentAmmo = new ReactiveProperty<int>();
            CurrentReloadTimer = new ReactiveProperty<float>();
        }

        public void ApplySettings(float reloadDuration, int maxAmmo)
        {
            ReloadDuration = reloadDuration;
            MaxAmmo = maxAmmo;
        }

        public bool TryShoot()
        {
            if (_currentAmmo.Value == 0)
            {
                return false;
            }

            _currentAmmo.Value--;
            return true;
        }

        public void TryAddAmmo(int count)
        {
            if (_currentAmmo.Value == MaxAmmo)
            {
                return;
            }

            _currentAmmo.Value = Mathf.Min(MaxAmmo, _currentAmmo.Value + count);
        }

        public void ResetAmmo()
        {
            _currentAmmo.Value = 0;
        }
    }
}