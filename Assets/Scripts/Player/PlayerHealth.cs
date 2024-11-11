using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer; // animates healthBar
    public float maxHealth = 100;
    //controls how quickly the delayed bar catches up    
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.A)) {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealthUI() {
        Debug.Log(health);
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillBack > hFraction) {
            // player took damage
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete; // square it to make animation smoother
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
        }

        if (fillFront < hFraction) {
            // player gained health
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete; // square it to make animation smoother
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, hFraction, percentComplete);
        }

    }

    public void TakeDamage(float damage) {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount) {
        health += healAmount;
        lerpTimer = 0f;
    }
}
