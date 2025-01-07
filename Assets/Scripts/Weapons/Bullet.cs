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
            hitTransform.GetComponent<Enemy>().TakeDamage(bulletDamage);
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

    void ShotgunImpactEffect(Collision objectHit)
    {
        // special effect for shotgun

        ContactPoint contactPoint = objectHit.contacts[0];

        GameObject effect = Instantiate(
            GlobalReferences.Instance.shotgunImpactEffect,
            contactPoint.point,
            Quaternion.LookRotation(contactPoint.normal)
        );

        effect.transform.SetParent(objectHit.gameObject.transform);
    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contactPoint = objectHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contactPoint.point,
            Quaternion.LookRotation(contactPoint.normal)
        );

        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}
