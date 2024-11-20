using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Basic properties for all weapons
    public GameObject bulletPrefab;         // Bullet prefab to instantiate
    public Transform gunbarrel;             // Where bullets are fired from
    public float bulletVelocity = 30f;      // Bullet velocity
    public float bulletLifeTime = 3f;       // How long the bullet exists
    public int ammoCount = 30;              // Ammo count for the gun (this will be specific to each weapon)
    public float fireRate = 0.1f;           // Rate of fire for the gun
    private bool isFiring = false;           // Is the weapon currently firing?
    private float nextFireTime = 0f;        // Time when we can fire next (used for fire rate control)

    // Called when the player starts firing the weapon
    public virtual void StartFire()
    {
        Debug.Log("StartFire called");
        if (Time.time >= nextFireTime) // Check if it's time to fire based on fire rate
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Called when the player stops firing
    public virtual void StopFire()
    {
        // Can be used for weapons with continuous fire (like machine guns)
        isFiring = false;
    }

    // The actual firing logic (will be overridden by specific weapon types)
    protected virtual void Fire()
    {
        Debug.Log("Base weapon fire logic.");
    }

    // Reloads the weapon
    public virtual void Reload()
    {
        Debug.Log("Reloading weapon...");
        ammoCount = 30;  // Reset ammo count to full
    }
}
