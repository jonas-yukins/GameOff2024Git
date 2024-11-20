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
        // Calculate the direction to the crosshair
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen (crosshair)
        RaycastHit hit;

        Vector3 targetPoint = ray.origin + ray.direction * 100f; // Default to a far distance if no hit occurs

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;  // Get the point where the ray hit
        }

        // Instantiate the bullet and set its position to the gun barrel
        GameObject bullet = Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);

        // Calculate the direction from the gun barrel to the target point
        Vector3 directionToTarget = (targetPoint - gunbarrel.position).normalized;

        // Apply force to the bullet towards the target
        bullet.GetComponent<Rigidbody>().AddForce(directionToTarget * bulletVelocity, ForceMode.Impulse);

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
