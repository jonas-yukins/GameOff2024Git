using UnityEngine;

public class Pistol : Weapon
{
    protected override void Fire()
    {
        base.Fire();  // Call base method (will check ammo and fire bullet)

        // Additional firing logic specific to Assault Rifle, if any
    }

    public override void Reload()
    {
        base.Reload();
        ammoCount = 12;  // Specific ammo count for the Assault Rifle
    }
}
