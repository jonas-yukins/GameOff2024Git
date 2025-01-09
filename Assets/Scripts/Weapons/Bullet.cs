using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    private void OnCollisionEnter(Collision objectHit)
    {
        Transform hitTransform = objectHit.transform;

        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
        }
        
        if (hitTransform.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");

            if (hitTransform.gameObject.GetComponent<Enemy>().isDead == false)
            {
                hitTransform.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }

            CreateBloodSprayEffect(objectHit);
        }
        
        if (hitTransform.CompareTag("Map"))
        {
            Debug.Log("Hit Map");
            CreateBulletImpactEffect(objectHit);

            if (WeaponManager.Instance.currentWeapon.currentShootingMode == Weapon.ShootingMode.Pellets)
            {
                ShotgunImpactEffect(objectHit);
            }
        }

        if (hitTransform.CompareTag("Bottle"))
        {
            var explodingBottle = objectHit.collider.GetComponent<Bottle>();
                
                if (explodingBottle != null)
                {
                    // Trigger the explosion
                    explodingBottle.Explode();
                    Debug.Log("Bottle exploded!");
                }
        }

        Destroy(gameObject);
    }

    private void CreateBloodSprayEffect(Collision objectHit)
    {
        ContactPoint contactPoint = objectHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contactPoint.point,
            Quaternion.LookRotation(contactPoint.normal)
        );

        bloodSprayPrefab.transform.SetParent(objectHit.gameObject.transform);
    }

    void ShotgunImpactEffect(Collision objectHit)
    {
        // special effect for shotgun

        ContactPoint contactPoint = objectHit.contacts[0];

        GameObject shotgunImpactPrefab = Instantiate(
            GlobalReferences.Instance.shotgunImpactEffect,
            contactPoint.point,
            Quaternion.LookRotation(contactPoint.normal)
        );

        shotgunImpactPrefab.transform.SetParent(objectHit.gameObject.transform);
    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contactPoint = objectHit.contacts[0];

        GameObject bulletHolePrefab = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contactPoint.point,
            Quaternion.LookRotation(contactPoint.normal)
        );

        bulletHolePrefab.transform.SetParent(objectHit.gameObject.transform);
    }
}
