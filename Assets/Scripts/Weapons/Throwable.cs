using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    public float ammoCount;
    public float throwForce = 10f;
    public GameObject grenadePrefab;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        Grenade
    }

    public ThrowableType throwableType;

    private void Awake()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        // Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            // apply damage to enemy here
        }
    }

    internal void ThrowLethal()
    {
        if (ammoCount < 1)
        {
            return;
        }

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
        Vector3 directionToTarget = (targetPoint - WeaponManager.Instance.currentWeapon.gunbarrel.position).normalized;

        // Instantiate the bullet at the gun barrel
        GameObject lethalPrefab = Instantiate(grenadePrefab, WeaponManager.Instance.currentWeapon.gunbarrel.position, WeaponManager.Instance.currentWeapon.gunbarrel.rotation);

        // Apply force to the bullet in the final direction
        lethalPrefab.GetComponent<Rigidbody>().AddForce(directionToTarget * throwForce, ForceMode.Impulse);
        hasBeenThrown = true;

        ammoCount -= 1;
    }
}
