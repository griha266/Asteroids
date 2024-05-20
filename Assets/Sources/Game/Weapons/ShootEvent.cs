namespace Asteroids.Game.Weapons
{
    public readonly struct ShootEvent
    {
        public readonly WeaponModel Weapon;

        public ShootEvent(WeaponModel weapon)
        {
            Weapon = weapon;
        }
    }
}