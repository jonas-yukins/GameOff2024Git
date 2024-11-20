using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;  // Reference to the current weapon
    public Transform cameraTransform;  // Reference to the camera (should be the main camera)

    // Weapon prefabs
    public GameObject AssaultRiflePrefab;  // Prefab of the Assault Rifle


    void Start()
    {
        cameraTransform = Camera.main.transform;  // Assign the camera transform
        EquipWeapon(AssaultRiflePrefab);  // Equip the initial weapon (Assault Rifle)
    }

    // Equip a new weapon and parent it to the camera
    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);  // Destroy the current weapon if it exists
        }

        // Instantiate the new weapon and set its parent to the camera
        GameObject weaponObject = Instantiate(newWeaponPrefab, cameraTransform);
        currentWeapon = weaponObject.GetComponent<Weapon>();  // Get the Weapon script from the instantiated object

        // Set the local position and rotation of the weapon relative to the camera
        weaponObject.transform.localPosition = new Vector3(0.328f, -0.359f, 0.985f);  // Adjust position
        weaponObject.transform.localRotation = Quaternion.Euler(0f, 6f, 0f);  // Adjust rotation
        //weaponObject.transform.localScale = new Vector3(0.0503717f, 05037171f, 0503717f); // Adjust size
    }

    // Start firing the current weapon
    public void StartFire()
    {
        if (currentWeapon != null)
        {
            currentWeapon.StartFire();  // Call the weapon's StartFire method
        }
    }

    // Stop firing the current weapon
    public void StopFire()
    {
        if (currentWeapon != null)
        {
            currentWeapon.StopFire();  // Call the weapon's StopFire method
        }
    }

    public void Reload()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Reload(); // Call the weapon's Reload method
        }
    }
}
