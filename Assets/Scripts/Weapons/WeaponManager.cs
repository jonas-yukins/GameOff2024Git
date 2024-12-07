using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;
    public Transform cameraTransform;

    private int currentWeaponIndex = 0;  // Start with the first weapon (Pistol)
    public List<GameObject> weaponPrefabs = new List<GameObject>();  // List of weapon prefabs

    void Start()
    {
        cameraTransform = Camera.main.transform;

        //Debug.Log(weaponPrefabs[0] + ", " + weaponPrefabs[1] + ", " + weaponPrefabs[2]);

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

        // set ammoCount = maxAmmo
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
        if (currentWeapon != null && currentWeapon.ammoCount != currentWeapon.maxAmmo)
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
}

