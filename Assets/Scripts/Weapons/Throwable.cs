using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    public float ammoCount;
    public float throwForce = 10f;
    public int damage = 100;

    // Generic prefab that can be any throwable object
    public GameObject throwablePrefab;

    private float countdown;

    private bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        Grenade,
        //Rock,
        //Molotov,
        // Add other types as needed
    }

    public ThrowableType throwableType;

    private void Awake()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        // Handle effects based on throwable type
        GetThrowableEffect();

        // Destroy the throwable after explosion (or action)
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            /* case ThrowableType.Rock:
                RockEffect();
                break;
            case ThrowableType.Molotov:
                MolotovEffect();
                break;
            // Add other throwable effects as needed
            */
        }
    }

    private void GrenadeEffect()
    {
        // Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Play Sound
        SoundManager.Instance.ItemChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                if (objectInRange.gameObject.GetComponent<Enemy>().isDead == false)
                {
                    objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
        }
    }

    private void RockEffect()
    {
        // Rock effect logic here
        // (Could be a simple impact effect, damage, or something else)
    }

    private void MolotovEffect()
    {
        // Molotov effect logic here
        // (Could be fire, burn damage, etc.)
    }

    internal void Throw()
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
        Vector3 throwPosition = cameraPosition + cameraForward * spawnDistance   // In front of the camera
                               + cameraRight * leftOffset                    // Slightly to the left
                               + cameraUp * upOffset;                         // Slightly up

        // Instantiate the generic throwable prefab at the calculated position
        GameObject throwableObject = Instantiate(throwablePrefab, throwPosition, Quaternion.identity);

        // Set the throwable object's Rigidbody to use gravity and apply throw force
        Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Ensure gravity is enabled for the throwable
            rb.AddForce(cameraForward * throwForce, ForceMode.Impulse); // Apply force in the forward direction
        }

        // Set flag to indicate the object has been thrown
        hasBeenThrown = true;

        // Decrease ammo count after throwing
        ammoCount -= 1;
    }
}
