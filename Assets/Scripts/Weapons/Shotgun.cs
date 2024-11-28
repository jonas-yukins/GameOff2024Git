using UnityEngine;

public class Shotgun : Weapon
{
    // Additional shotgun properties
    public int pelletsPerShot = 5;  // Number of pellets fired per shot
    public float spreadAngle = 0.05f;  // The spread angle for shotgun pellets

    // Override the Fire method to handle shotgun behavior
    protected override void Fire()
    {
        if (ammoCount > 0)
        {
            // Animations Effect
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            animator.SetTrigger("RECOIL");

            SoundManager.Instance.shootingSound1911.Play();

            // Fire multiple pellets with random spread
            for (int i = 0; i < pelletsPerShot; i++)
            {
                FirePellet();
            }

            ammoCount--;  // Decrease ammo after firing
            Debug.Log($"Ammo left: {ammoCount}");
        }
        else
        {
            Debug.Log("Out of ammo!");
            SoundManager.Instance.emptyMagazineSound1911.Play();
            //Reload(); // Auto reload if out of ammo
        }
    }

    // Method to fire a single pellet with spread
    private void FirePellet()
    {
        // Calculate random spread for each pellet
        Vector3 spreadOffset = new Vector3(
            Random.Range(-spreadAngle, spreadAngle),  // Random X spread
            Random.Range(-spreadAngle, spreadAngle),  // Random Y spread
            Random.Range(-spreadAngle, spreadAngle)   // Random Z spread
        );

        // Direction to target - can be any direction, but we're using forward for simplicity
        Vector3 finalDirection = gunbarrel.forward + spreadOffset;

        // Instantiate the pellet at the gun barrel
        GameObject pellet = Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);

        // Apply force to the pellet in the final direction
        Rigidbody pelletRb = pellet.GetComponent<Rigidbody>();
        if (pelletRb != null)
        {
            pelletRb.AddForce(finalDirection.normalized * bulletVelocity, ForceMode.Impulse);
        }

        // Destroy the pellet after a set time
        Destroy(pellet, bulletLifeTime);
    }

    public override void Reload()
    {
        base.Reload();
    }
}
