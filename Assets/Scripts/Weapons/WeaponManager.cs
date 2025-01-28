using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }  // Singleton Instance

    public Transform cameraTransform;
    public Transform weaponHolder;
    
    [Header("Throwables")]
    public Throwable currentThrowable;
    private int currentThrowableIndex = 0; // Start with first throwable (grenade)
    public List<GameObject> throwablePrefabs = new List<GameObject>();  // List of lethal prefabs

    [Header("Tacticals")]
    public Tactical currentTactical;
    private int currentTacticalIndex = 0; // Start with first tactical (grenade)
    public List<GameObject> tacticalPrefabs = new List<GameObject>();  // List of tactical prefabs

    [Header("Weapons")]
    public Weapon currentWeapon;
    private int currentWeaponIndex = 0;  // Start with the first weapon (Pistol)
    public List<GameObject> weaponPrefabs = new List<GameObject>();  // List of weapon prefabs

    [Header("IK Hand Targets")]
    public Animator armAnimator;
    // Reference to the TwoBoneIKConstraint components
    public TwoBoneIKConstraint rightHandIKConstraint;
    public TwoBoneIKConstraint leftHandIKConstraint;
    public RigBuilder rigBuilder;

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
        EquipTactical(tacticalPrefabs[currentTacticalIndex]); // Default to the first tactical
        EquipThrowable(throwablePrefabs[currentThrowableIndex]);  // Default to the first throwable
    }

    private void EquipTactical(GameObject newTactical)
    {
        currentTactical = newTactical.GetComponent<Tactical>();
        currentTactical.tacticalPrefab = newTactical;
        currentTactical.ammoCount = 2;
    }

    private void EquipThrowable(GameObject newThrowable)
    {
        currentThrowable = newThrowable.GetComponent<Throwable>();
        currentThrowable.throwablePrefab = newThrowable;
        currentThrowable.ammoCount = 2;
    }

    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);  // Destroy the previous weapon if exists
        }

        // Instantiate the new weapon
        GameObject weaponObject = Instantiate(newWeaponPrefab, weaponHolder);
        currentWeapon = weaponObject.GetComponent<Weapon>();  // Get the Weapon component

        // Update IK targets
        UpdateIKTargets();

        // Optional: Delay reload to make sure everything initializes properly
        StartCoroutine(DelayedReload());
    }


    private void UpdateIKTargets()
    {
        // Now assign the weapon's grip points to the IK targets
        rightHandIKConstraint.data.target = currentWeapon.ref_right_hand_grip;
        leftHandIKConstraint.data.target = currentWeapon.ref_left_hand_grip;

        // Update Rig
        rigBuilder.Build();
    }

    private IEnumerator DelayedReload()
    {
        yield return null;  // Wait for the next frame

        currentWeapon.Reload();  // Now reload
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
            currentWeapon.isADS = false;
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

        currentThrowable.ammoCount += 2;
        currentTactical.ammoCount += 2;
    }

    public void handleADS()
    {
        if (currentWeapon != null)
        {
            currentWeapon.toggleADS();
        }
    }

    internal void handleThrowable()
    {
        if (currentThrowable != null)
        {
            currentThrowable.Throw();
        }
    }

    internal void handleTactical()
    {
        if (currentTactical != null)
        {
            currentTactical.applyTactical();
        }
    }
}