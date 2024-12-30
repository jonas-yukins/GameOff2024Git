using System;
using UnityEngine;

public class Tactical : MonoBehaviour
{
    // Reference to the Player
    public GameObject player;
    private PlayerHealth playerHealth;

    public float ammoCount;

    // Generic prefab that can be any tactical object
    public GameObject tacticalPrefab;

    public enum TacticalType
    {
        Pills
        // Add other types as needed
    }

    public TacticalType tacticalType;

    private void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        
    }

    public void applyTactical()
    {
        if (ammoCount < 1)
        {
            return;
        }

        // Get the camera's position and direction
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraUp = Camera.main.transform.up;

        // Define how far the throwable object should spawn in front of the camera and apply offsets
        float spawnDistance = 1.0f; // How far in front of the camera the throwable will spawn
        float leftOffset = -0.5f;  // Move a little to the left (negative)
        float upOffset = 0.3f;     // Move a little up

        // Calculate the spawn position
        Vector3 spawnPosition = cameraPosition + cameraForward * spawnDistance   // In front of the camera
                               + cameraRight * leftOffset                    // Slightly to the left
                               + cameraUp * upOffset;                         // Slightly up

        // Instantiate the generic throwable prefab at the calculated position
        Instantiate(tacticalPrefab, spawnPosition, Quaternion.identity);

        switch (tacticalType)
        {
            case TacticalType.Pills:
                PillsEffect();
                break;
            // Add other throwable effects as needed
        }

        //Destroy(gameObject);
    }

    private void PillsEffect()
    {
        Debug.Log("PillsEffect");
        //playerHealth.RestoreHealth(20);
    }
}
