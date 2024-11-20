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
    public float fireRate = 0.1f;           // Rate of fire for the gun
    private bool isFiring = false;           // Is the weapon currently firing?
    private float nextFireTime = 0f;        // Time when we can fire next (used for fire rate control)
    public int maxAmmo = 30;    // Max ammo capacity
    public int ammoCount;  // Current ammo count

    public float reloadTime = 2f;  // Time it takes to reload
    private bool isReloading = false;  // Flag to prevent multiple reloads at once


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
        if (isReloading)
            return;  // Don't reload if already reloading

        isReloading = true;
        while (isReloading) {
            StopFire(); // can't shoot while reloading
        }
        Debug.Log("Reloading...");

        // Start reloading process
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        // Simulate reload time (e.g., playing reload animation)
        yield return new WaitForSeconds(reloadTime);

        // Reload the weapon
        ammoCount = maxAmmo;
        isReloading = false;

        Debug.Log("Reloaded! Ammo: " + ammoCount);
    }
}
