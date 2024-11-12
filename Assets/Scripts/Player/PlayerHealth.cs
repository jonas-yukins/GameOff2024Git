using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer; // animates healthBar
    [Header("Health Bar")]
    public float maxHealth = 100;
    //controls how quickly the delayed bar catches up    
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TMP_Text healthText;

    [Header("Damage Overlay")]
    public Image overlay; // DamageOverlay GameObject
    public float duration; // how long image stays fully opaque
    public float fadeSpeed;

    private float durationTimer;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateHealthText();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        UpdateHealthText();

        if (overlay.color.a > 0) {
            if (health < 30) {
                return; // don't fade if health is below 30
            }
            durationTimer += Time.deltaTime;
            if (durationTimer > duration) {
                // fade image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
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
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount) {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void UpdateHealthText() {
        if (healthText != null) {
            healthText.text = health.ToString() + "/100";
        }
    }
}
