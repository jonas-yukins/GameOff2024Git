using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;
    public Transform cameraTransform;
    public GameObject AssaultRiflePrefab;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        EquipWeapon(AssaultRiflePrefab);  // Equip initial weapon
    }

    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);  // Destroy current weapon if exists
        }

        GameObject weaponObject = Instantiate(newWeaponPrefab, cameraTransform);
        currentWeapon = weaponObject.GetComponent<Weapon>();  // Get the Weapon component
        SetWeaponPositionAndRotation(weaponObject);
    }

    private void SetWeaponPositionAndRotation(GameObject weaponObject)
    {
        weaponObject.transform.localPosition = new Vector3(0.328f, -0.359f, 0.985f);  // Position adjustment
        weaponObject.transform.localRotation = Quaternion.Euler(0f, 6f, 0f);  // Rotation adjustment
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
