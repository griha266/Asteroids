using System;
using Asteroids.Core.Systems;
using Asteroids.Utils;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Asteroids.Game.Weapons
{
    public class WeaponInputsSystem : IInitializeable, IDisposable
    {
        private readonly WeaponModel _laserModel;
        private readonly WeaponModel _regularWeaponModel;
        private readonly InputAction _laserInputAction;
        private readonly InputAction _regularWeaponInputAction;
        private readonly EventBus<ShootEvent> _shootEvents;

        public WeaponInputsSystem(
            WeaponModel laserModel, 
            WeaponModel regularWeaponModel,
            InputAction laserInputAction, 
            InputAction regularWeaponInputAction, 
            EventBus<ShootEvent> shootEvents
        ) {
            _laserInputAction = laserInputAction;
            _regularWeaponInputAction = regularWeaponInputAction;
            _shootEvents = shootEvents;
            _laserModel = laserModel;
            _regularWeaponModel = regularWeaponModel;
        }

        public void Init()
        {
            _laserInputAction.performed += OnLaserInputEvent;
            _regularWeaponInputAction.performed += OnRegularWeaponInputEvent;
        }

        private void OnRegularWeaponInputEvent(InputAction.CallbackContext obj)
        {
            ShootWeapon(_regularWeaponModel);
        }

        private void OnLaserInputEvent(InputAction.CallbackContext obj)
        {
            ShootWeapon(_laserModel);
        }

        private void ShootWeapon(WeaponModel weapon)
        {
            if (weapon.TryShoot())
            {
                Debug.Log("Shoot weapon");
                _shootEvents.Push(new ShootEvent(weapon));
            }
        }


        public void Dispose()
        {
            _laserInputAction.performed -= OnLaserInputEvent;
            _regularWeaponInputAction.performed -= OnRegularWeaponInputEvent;
        }
    }
}