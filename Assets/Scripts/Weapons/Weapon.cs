using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunbarrel;
    public float bulletVelocity = 30f;
    public float bulletLifeTime = 3f;
    public float fireRate = 0.1f;
    private bool isFiring = false;
    private float nextFireTime = 0f;
    public int maxAmmo = 30;
    public int ammoCount;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public virtual void StartFire()
    {
        if (isReloading)
        {
            Debug.Log("Cannot fire while reloading.");
            return;
        }

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    public virtual void StopFire()
    {
        isFiring = false;
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

            // Instantiate the bullet at the gun barrel
            GameObject bullet = Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);

            // Calculate direction from the gun barrel to the target point
            Vector3 directionToTarget = (targetPoint - gunbarrel.position).normalized;

            // Apply force to the bullet in that direction
            bullet.GetComponent<Rigidbody>().AddForce(directionToTarget * bulletVelocity, ForceMode.Impulse);

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
