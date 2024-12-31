using System.Collections;
using UnityEngine;

public class Tactical : MonoBehaviour
{
    public float ammoCount;
    public GameObject tacticalPrefab;

    private GameObject currentTacticalObject;  // Store reference to the current tactical object

    public enum TacticalType
    {
        Pills
        // Add other types as needed
    }

    public TacticalType tacticalType;

    public void applyTactical()
    {
        if (ammoCount <= 0)
        {
            Debug.Log("No ammo left for tactical item!");
            return;
        }

        ammoCount--;  // Decrease ammo count

        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found.");
            return;
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraUp = Camera.main.transform.up;

        float spawnDistance = 1.0f;
        float leftOffset = -0.5f;
        float upOffset = 0.3f;

        Vector3 spawnPosition = cameraPosition + cameraForward * spawnDistance
                               + cameraRight * leftOffset
                               + cameraUp * upOffset;

        currentTacticalObject = Instantiate(tacticalPrefab, spawnPosition, Quaternion.identity);
        currentTacticalObject.transform.SetParent(Camera.main.transform);

        switch (tacticalType)
        {
            case TacticalType.Pills:
                PillsEffect(currentTacticalObject);
                // Play Sound
                SoundManager.Instance.tacticalsChannel.PlayOneShot(SoundManager.Instance.pillBottleSound);
                break;
            // Handle other tactical effects if needed
        }
    }

    private void PillsEffect(GameObject spawnedTactical)
    {
        Debug.Log("PillsEffect");
    }

    private void DestroyTactical()
    {
        // Prompted by animation event
        Debug.Log("Destroy Tactical");
        // Optionally, you can add any cleanup here (effects, sounds, etc.)
        Destroy(gameObject);  // Destroy the tactical object
        
    }

    private void HealEffect(int input)
    {
        // Prompted by animation event
        PlayerHealth.Instance.RestoreHealth(input);
    }
}
