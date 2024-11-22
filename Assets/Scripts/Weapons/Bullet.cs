using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectHit)
    {
        Transform hitTransform = objectHit.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
        } else if (hitTransform.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            // decrease AI health
            //hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
        } else if (hitTransform.CompareTag("Map"))
        {
            Debug.Log("Hit Map");
            CreateBulletImpactEffect(objectHit);
        }
        Destroy(gameObject);
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
