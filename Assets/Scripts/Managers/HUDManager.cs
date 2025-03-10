using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;


    [Header("Weapon")]
    public Image activeWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

    public GameObject middleDot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.currentWeapon;
        Throwable activeThrowable = WeaponManager.Instance.currentThrowable;
        Tactical activeTactical = WeaponManager.Instance.currentTactical;

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.ammoCount}";
            totalAmmoUI.text = $"{activeWeapon.totalAmmo}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
        }

        if (activeThrowable)
        {
            lethalAmountUI.text = $"{activeThrowable.ammoCount}";
            Throwable.ThrowableType throwableType = activeThrowable.throwableType;
            lethalUI.sprite = GetThrowableSprite(throwableType);
        } else
        {
            lethalAmountUI.text = "";
            lethalUI.sprite = emptySlot;
        }

        if (activeTactical)
        {
            tacticalAmountUI.text = $"{activeTactical.ammoCount}";
            Tactical.TacticalType tacticalType = activeTactical.tacticalType;
            tacticalUI.sprite = GetTacticalSprite(tacticalType);
        } else
        {
            lethalAmountUI.text = "";
            lethalUI.sprite = emptySlot;
        }
    }

    private Sprite GetTacticalSprite(Tactical.TacticalType tactical)
    {
        switch (tactical)
        {
            case Tactical.TacticalType.Pills:
                return Resources.Load<GameObject>("HealthPack").GetComponent<SpriteRenderer>().sprite;
            
            default:
                return null;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            
            case Weapon.WeaponModel.AssaultRifle:
                return Resources.Load<GameObject>("AK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            
            case Weapon.WeaponModel.Shotgun:
                return Resources.Load<GameObject>("Shotgun_Weapon").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            
            case Weapon.WeaponModel.AssaultRifle:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;
            
            case Weapon.WeaponModel.Shotgun:
                return Resources.Load<GameObject>("Shotgun_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetThrowableSprite(Throwable.ThrowableType throwable)
    {
        switch (throwable)
        {
            case Throwable.ThrowableType.Grenade:
                return Resources.Load<GameObject>("Grenade_Throwable").GetComponent<SpriteRenderer>().sprite;
            
            default:
                return null;
        }
    }
}
