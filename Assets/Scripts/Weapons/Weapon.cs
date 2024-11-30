using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Pellets
    public int pelletsPerShot = 15;  // Number of pellets fired per shot
    private int currentPellets;

    // Spread
    public float spreadIntensity = 0.01f;

    // Animation
    public GameObject muzzleEffect;
    public Animator animator;

    public enum WeaponModel
    {
        Pistol,
        AssaultRifle,
        Shotgun
    }

    public WeaponModel thisWeaponModel;


    // Shooting Modes
    public enum ShootingMode
    {
        Single,
        Burst,
        Pellets,
        Auto
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{ammoCount}/{maxAmmo}";
        }
    }

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
            else if (currentShootingMode == ShootingMode.Pellets)
            {
                // Shotgun Mode: Fire a set number of bullets
                if (!isFiring)
                {
                    StartCoroutine(FirePellets());
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
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            animator.SetTrigger("RECOIL");

            SoundManager.Instance.PlayShootingSound(thisWeaponModel);

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

            if (currentShootingMode != ShootingMode.Pellets) {
                // if gun is on Pellet mode, ammo is handled differently
                ammoCount--;  // Decrease ammo
            }
            Debug.Log($"Ammo left: {ammoCount}");
        }
        else
        {
            Debug.Log("Out of ammo!");
            SoundManager.Instance.emptyMagazineSound.Play(); // empty magazine sound
            // Reload(); // Auto reload if out of ammo
        }
    }


    private IEnumerator FireAuto()
    {
        while (isFiring)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);  // Delay between automatic shots
        }
    }

    private IEnumerator FireBurst()
    {
        isFiring = true;
        currentBurst = bulletPerBurst;

        while (currentBurst > 0)
        {
            Fire();
            currentBurst--;
            yield return new WaitForSeconds(fireRate);  // Delay between burst shots
        }

        isFiring = false;
    }

    private IEnumerator FirePellets()
    {
        isFiring = true;
        currentPellets = pelletsPerShot;
        
        while (currentPellets > 0)
        {
            Fire();
            currentPellets--;
        }

        if (ammoCount > 0) {
            ammoCount--;
        }

        yield return new WaitForSeconds(fireRate);
        isFiring = false;
    }

    public virtual void Reload()
    {
        if (isReloading) {
            return;
        }

        isReloading = true;
        StopFire();

        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        ammoCount = maxAmmo; // reset ammo count
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
