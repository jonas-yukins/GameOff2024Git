using UnityEngine;

public class Shotgun : Weapon
{
    // Additional shotgun properties
    public int pelletsPerShot = 15;  // Number of pellets fired per shot
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
        if (ammoCount > 0)
        {
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            animator.SetTrigger("RECOIL");

            SoundManager.Instance.shootingSound1911.Play();

            // Fire towards the crosshair (center of the screen)
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen
            RaycastHit hit;
            Vector3 targetPoint = ray.origin + ray.direction * 100f; // Default to a far distance if no hit occurs

            // If the ray hits something, we use the hit point as the target point
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point; 
            }

            // Calculate direction from the gun barrel to the target point
            Vector3 directionToTarget = (targetPoint - gunbarrel.position).normalized;

            // Calculate random spread for each pellet
            Vector3 spreadOffset = new Vector3(
                Random.Range(-spreadAngle, spreadAngle),  // Random X spread
                Random.Range(-spreadAngle, spreadAngle),  // Random Y spread
                Random.Range(-spreadAngle, spreadAngle)   // Random Z spread
            );

            // Apply the spread offset to the direction vector
            Vector3 finalDirection = directionToTarget + spreadOffset;

            // Normalize the direction to keep the bullet speed constant
            finalDirection.Normalize();

            // Instantiate the bullet at the gun barrel
            GameObject bullet = Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);

            // Apply force to the bullet in the final direction
            bullet.GetComponent<Rigidbody>().AddForce(finalDirection * bulletVelocity, ForceMode.Impulse);

            // Destroy the bullet after a set time
            Destroy(bullet, bulletLifeTime);

            Debug.Log($"Ammo left: {ammoCount}");
        }
    }

    public override void Reload()
    {
        base.Reload();
    }
}
