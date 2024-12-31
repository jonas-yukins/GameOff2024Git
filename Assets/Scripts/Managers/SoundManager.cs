using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    // Shooting
    public AudioSource ShootingChannel;
    public AudioClip P1911Shot;
    public AudioClip AK47Shot;
    public AudioClip ShotgunShot;

    // Reloading
    public AudioSource ReloadingChannel;
    public AudioClip P1911Reload;
    public AudioClip AK47Reload;
    public AudioClip ShotgunReload;
    public AudioClip emptyMagazineSound;
    public AudioClip AmmoBoxSound;

    // Throwables
    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    // Tacticals
    public AudioSource tacticalsChannel;
    public AudioClip pillBottleSound;

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
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case Weapon.WeaponModel.AssaultRifle:
                // Assault Rifle
                ShootingChannel.PlayOneShot(AK47Shot);
                break;
            case Weapon.WeaponModel.Shotgun:
                // Shotgun
                ShootingChannel.PlayOneShot(ShotgunShot);
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                // Pistol
                ReloadingChannel.PlayOneShot(P1911Reload);
                break;
            case Weapon.WeaponModel.AssaultRifle:
                // Assault Rifle
                ReloadingChannel.PlayOneShot(AK47Reload);
                break;
            case Weapon.WeaponModel.Shotgun:
                // Shotgun
                ReloadingChannel.PlayOneShot(ShotgunReload);
                break;
        }
    }

    public void PlayEmptyMagazineSound()
    {
        ReloadingChannel.PlayOneShot(emptyMagazineSound);
    }

    public void PlayAmmoBoxSound()
    {
        ReloadingChannel.PlayOneShot(AmmoBoxSound);
    }
}
