using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHand : MonoBehaviour
{
    public int damage;
    public Collider handCollider;

    // Called when the swing begins (Animation Event)
    public void EnableHandCollider()
    {
        if (handCollider != null)
        {
            handCollider.enabled = true;
        }
    }

    // Called when the swing ends (Animation Event)
    public void DisableHandCollider()
    {
        if (handCollider != null)
        {
            handCollider.enabled = false;
        }
    }
}
