using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerInput controls;
    public GameObject bulletPrefab;
    public Transform gunbarrel;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShot()
    {
        Debug.Log("PlayerShoot");

        // instantiate the bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet1") as GameObject, gunbarrel.position, gunbarrel.rotation);
        // Adjust the bullet's rotation to match the shoot direction
        bullet.transform.rotation = Quaternion.FromToRotation(bullet.transform.up, gunbarrel.forward.normalized);

        // shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(gunbarrel.forward.normalized * bulletVelocity, ForceMode.Impulse);
        
        // destory the bullet after some time
        Destroy(bullet, bulletPrefabLifeTime);
    }

    public void EndShot()
    {

    }
}
