using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;
    public Transform cameraTransform;

    // Weapons
    public GameObject AssaultRiflePrefab;
    public GameObject PistolPrefab;
    public GameObject ShotgunPrefab;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        EquipWeapon(ShotgunPrefab);  // Equip initial weapon
    }

    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);  // Destroy current weapon if exists
        }

        GameObject weaponObject = Instantiate(newWeaponPrefab, cameraTransform);
        currentWeapon = weaponObject.GetComponent<Weapon>();  // Get the Weapon component
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
        if (currentWeapon != null)
        {
            currentWeapon.Reload();  // Reload the current weapon
        }
    }
}
