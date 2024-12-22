using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }  // Singleton Instance

    public Weapon currentWeapon;
    public Transform cameraTransform;

    private int currentWeaponIndex = 0;  // Start with the first weapon (Pistol)
    public List<GameObject> weaponPrefabs = new List<GameObject>();  // List of weapon prefabs

    void Awake()
    {
        // Ensure only one instance of WeaponManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy any duplicate WeaponManager
        }
        else
        {
            Instance = this;  // Set this instance as the singleton
        }

        cameraTransform = Camera.main.transform;

        EquipWeapon(weaponPrefabs[currentWeaponIndex]);  // Default to the first weapon
    }

    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);  // Destroy current weapon if exists
        }

        GameObject weaponObject = Instantiate(newWeaponPrefab, cameraTransform);
        currentWeapon = weaponObject.GetComponent<Weapon>();  // Get the Weapon component

        // set ammoCount = magazineSize
        // plays animation
        currentWeapon.Reload();
    }

    public void HandleFire(bool isFiring)
    {
        if (currentWeapon != null)
        {
            if (isFiring)
                currentWeapon.StartFire();
            else
                currentWeapon.StopFire();
        }
    }

    public void Reload()
    {
        if (currentWeapon != null && currentWeapon.ammoCount != currentWeapon.magazineSize && currentWeapon.totalAmmo > 0)
        {
            currentWeapon.Reload();  // Reload the current weapon
        }
    }

    // Method to switch weapons based on an index
    public void SwitchWeapon(int weaponIndex)
    {
        // Ensure the index is valid and within the bounds of the weapon list
        if (weaponIndex >= 0 && weaponIndex < weaponPrefabs.Count)
        {
            EquipWeapon(weaponPrefabs[weaponIndex]);
            Debug.Log($"Switched to {weaponPrefabs[weaponIndex].name}");
        }
    }

    public void PickupAmmo()
    {
        switch (currentWeapon)
        {
            case Pistol:
                currentWeapon.totalAmmo += 48;
                break;
            case AssaultRifle:
                currentWeapon.totalAmmo += 120;
                break;
            case Shotgun:
                currentWeapon.totalAmmo += 18;
                break;
        }
    }
}

