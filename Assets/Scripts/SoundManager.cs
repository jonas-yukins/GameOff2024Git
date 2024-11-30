using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    // Pistol
    public AudioSource shootingSound1911;
    public AudioSource reloadingSound1911;

    // Assault Rifle
    public AudioSource shootingSoundAK47;
    public AudioSource reloadingSoundAK47;

    // Shotgun
    public AudioSource shootingSoundShotgun;
    public AudioSource reloadingSoundShotgun;


    public AudioSource emptyMagazineSound;

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

    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                // Pistol
                shootingSound1911.Play();
                break;
            case Weapon.WeaponModel.AssaultRifle:
                // Assault Rifle
                shootingSoundAK47.Play();
                break;
            case Weapon.WeaponModel.Shotgun:
                // Shotgun
                shootingSoundShotgun.Play();
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                // Pistol
                reloadingSound1911.Play();
                break;
            case Weapon.WeaponModel.AssaultRifle:
                // Assault Rifle
                reloadingSoundAK47.Play();
                break;
            case Weapon.WeaponModel.Shotgun:
                // Shotgun
                reloadingSoundShotgun.Play();
                break;
        }
    }
}
