using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    // Audio Channels
    [Header("Audio Channels")]
    public AudioSource WeaponChannel;
    public AudioSource ItemChannel;
    public AudioSource ZombieChannel;
    public AudioSource ZombieChannel2; // for hurt sound
    public AudioSource PlayerChannel;
    public AudioSource MusicChannel;

    // Shooting
    [Header("Shooting Sounds")]
    public AudioClip P1911Shot;
    public AudioClip AK47Shot;
    public AudioClip ShotgunShot;

    // Reloading
    [Header("Reloading Sounds")]
    public AudioClip P1911Reload;
    public AudioClip AK47Reload;
    public AudioClip ShotgunReload;
    public AudioClip emptyMagazineSound;

    // Interactables
    [Header("Interactable Sounds")]
    public AudioClip AmmoBoxSound;
    
    // Throwables
    [Header("Throwable Sounds")]
    public AudioClip grenadeSound;

    // Tacticals
    [Header("Tactical Sounds")]
    public AudioClip pillBottleSound;

    // Zombie
    [Header("Zombie Sounds")]
    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    // Player
    [Header("Player Sounds")]
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    // Music
    [Header("Music")]
    public AudioClip gameOverMusic;
    public AudioClip mainMenuMusic;


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

        DontDestroyOnLoad(this);
    }

    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                // Pistol
                WeaponChannel.PlayOneShot(P1911Shot);
                break;
            case Weapon.WeaponModel.AssaultRifle:
                // Assault Rifle
                WeaponChannel.PlayOneShot(AK47Shot);
                break;
            case Weapon.WeaponModel.Shotgun:
                // Shotgun
                WeaponChannel.PlayOneShot(ShotgunShot);
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                // Pistol
                WeaponChannel.PlayOneShot(P1911Reload);
                break;
            case Weapon.WeaponModel.AssaultRifle:
                // Assault Rifle
                WeaponChannel.PlayOneShot(AK47Reload);
                break;
            case Weapon.WeaponModel.Shotgun:
                // Shotgun
                WeaponChannel.PlayOneShot(ShotgunReload);
                break;
        }
    }

    public void PlayEmptyMagazineSound()
    {
        WeaponChannel.PlayOneShot(emptyMagazineSound);
    }

    public void PlayAmmoBoxSound()
    {
        ItemChannel.PlayOneShot(AmmoBoxSound);
    }
}
