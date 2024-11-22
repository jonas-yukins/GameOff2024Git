using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Object references
    public Transform gunbarrel;

    // Bullet
    public GameObject bulletPrefab;
    public float bulletVelocity = 30f;
    public float bulletLifeTime = 3f;
    public int maxAmmo = 30;
    public int ammoCount;

    // Gun
    public float fireRate = 0.1f;  
    private float nextFireTime = 0f;   
    public float reloadTime = 2f;

    // Current actions
    private bool isFiring = false;
    private bool isReloading = false;

    // Burst
    public int bulletPerBurst = 3;
    public int currentBurst;

    // Spread
    public float spreadIntensity = 0.01f;

    // Shooting Modes
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    public virtual void StartFire()
    {
        if (isReloading)
        {
            Debug.Log("Cannot fire while reloading.");
            return;
        }

        if (Time.time >= nextFireTime)
        {
            if (currentShootingMode == ShootingMode.Auto) 
            {
                // Automatic Fire: Continuously fire while the trigger is held
                if (!isFiring)
                {
                    isFiring = true;
                    StartCoroutine(FireAuto());
                }
            } 
            else if (currentShootingMode == ShootingMode.Burst) 
            {
                // Burst Mode: Fire a set number of bullets
                if (!isFiring)
                {
                    StartCoroutine(FireBurst());
                }
            } 
            else 
            {
                // Single Shot
                Fire();
            }
            
            nextFireTime = Time.time + fireRate;
        }
    }

    public virtual void StopFire()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isFiring = false;  // Stop continuous auto-fire
        }
    }

    protected virtual void Fire()
{
    if (ammoCount > 0)
    {
        // Fire towards the crosshair (center of the screen)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen
        RaycastHit hit;
        Vector3 targetPoint = ray.origin + ray.direction * 100f; // Default to a far distance if no hit occurs

        // If the ray hits something, we use the hit point as the target point
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; 
        }

        // Calculate direction from the gun barrel to the target point
        Vector3 directionToTarget = (targetPoint - gunbarrel.position).normalized;

        // Apply random spread
        Vector3 spreadOffset = new Vector3(
            Random.Range(-spreadIntensity, spreadIntensity),  // Random X spread
            Random.Range(-spreadIntensity, spreadIntensity),  // Random Y spread
            Random.Range(-spreadIntensity, spreadIntensity)   // Random Z spread
        );

        // Apply the spread offset to the direction vector
        Vector3 finalDirection = directionToTarget + spreadOffset;

        // Normalize the direction to keep the bullet speed constant
        finalDirection.Normalize();

        // Instantiate the bullet at the gun barrel
        GameObject bullet = Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);

        // Apply force to the bullet in the final direction
        bullet.GetComponent<Rigidbody>().AddForce(finalDirection * bulletVelocity, ForceMode.Impulse);

        // Destroy the bullet after a set time
        Destroy(bullet, bulletLifeTime);

        ammoCount--;  // Decrease ammo
        Debug.Log($"Ammo left: {ammoCount}");
    }
    else
    {
        Debug.Log("Out of ammo!");
    }
}


    private IEnumerator FireAuto()
    {
        while (isFiring && ammoCount > 0)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);  // Delay between automatic shots
        }
    }

    private IEnumerator FireBurst()
    {
        isFiring = true;
        currentBurst = bulletPerBurst;

        while (currentBurst > 0 && ammoCount > 0)
        {
            Fire();
            currentBurst--;
            yield return new WaitForSeconds(fireRate);  // Delay between burst shots
        }

        isFiring = false;
    }

    public virtual void Reload()
    {
        if (isReloading)
            return;

        isReloading = true;
        StopFire();
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        ammoCount = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
