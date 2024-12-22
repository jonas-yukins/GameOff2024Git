using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    protected override void Interact() {
        // Ammo boxes should only be able to be opened once

        // Play sound
        SoundManager.Instance.PlayAmmoBoxSound();

        // Play animation to open the box
        GetComponent<Animator>().SetBool("IsOpen", true);

        // Pickup ammo from the box
        WeaponManager.Instance.PickupAmmo();

        // Remove interaction prompt
        promptMessage = "";

        // Make the box no longer interactable
        DisableInteraction();
    }

    // Method to disable further interaction
    private void DisableInteraction() {
        // Disable the collider so it no longer receives interactions
        Collider collider = GetComponent<Collider>();
        if (collider != null) {
            collider.enabled = false;
        }

        // Optionally disable the outline (if you are using one)
        Outline outline = GetComponent<Outline>();
        if (outline != null) {
            outline.enabled = false;
        }

        // Optionally, you can also disable the interactable script itself to fully remove any interaction logic.
        this.enabled = false;
    }
}
