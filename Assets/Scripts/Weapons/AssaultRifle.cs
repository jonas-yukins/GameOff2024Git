using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
{
    // Override the fire method for the assault rifle
    protected override void Fire()
    {
        base.Fire();  // Optionally call base implementation if desired

        if (ammoCount > 0)
        {
            // Instantiate the bullet and apply force
            GameObject bullet = Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(gunbarrel.forward * bulletVelocity, ForceMode.Impulse);

            // Destroy the bullet after some time
            Destroy(bullet, bulletLifeTime);

            ammoCount--;  // Decrease ammo
            Debug.Log("Assault Rifle fired! Ammo left: " + ammoCount);
        }
        else
        {
            Debug.Log("Out of ammo!");
        }
    }

    // Override reload logic if needed
    public override void Reload()
    {
        base.Reload();  // Calls the base reload method
        ammoCount = 30; // Set to full ammo for this specific weapon
    }
}
