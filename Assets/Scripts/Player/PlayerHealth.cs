using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    private float health;
    private float lerpTimer; // Animates healthBar
    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TMP_Text healthText;

    [Header("Screen Overlays")]
    public Image damageOverlay;  // DamageOverlay Image
    public Image healOverlay;    // HealOverlay Image
    public float duration;       // How long image stays fully opaque
    public float fadeSpeed;

    private float durationTimer;
    private bool isDamageOverlayActive = false;
    private bool isHealOverlayActive = false;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy duplicate instance
        }
        else
        {
            Instance = this;  // Set this instance as the singleton
        }

        health = maxHealth;
        UpdateHealthText();
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0); // Transparent by default
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0); // Transparent by default
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        UpdateHealthText();

        // Handle fading of overlays
        if (isDamageOverlayActive)
        {
            FadeOverlay(damageOverlay);
        }
        else if (isHealOverlayActive)
        {
            FadeOverlay(healOverlay); 
        }
    }

    // Function to fade the overlay based on alpha value
    void FadeOverlay(Image overlay)
    {
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                // Reduce alpha over time
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Clamp(tempAlpha, 0, 1)); // Ensure alpha doesn't go below 0
            }
        }
    }

    public void UpdateHealthUI()
    {
        Debug.Log(health);
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillBack > hFraction)
        {
            // Player took damage
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete; // square it to make animation smoother
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);

            // Activate damage overlay
            ShowDamageOverlay();
        }

        if (fillFront < hFraction)
        {
            // Player gained health
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete; // square it to make animation smoother
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, hFraction, percentComplete);

            // Activate heal overlay
            ShowHealOverlay();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;

        // Hide heal overlay if active
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0);
        isHealOverlayActive = false;

        // Reset or reactivate damage overlay
        ShowDamageOverlay();
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
        durationTimer = 0;

        // Hide damage overlay if active
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
        isDamageOverlayActive = false;

        // Activate heal overlay
        ShowHealOverlay();
    }

    // Function to show damage overlay
    void ShowDamageOverlay()
    {
        // Always reset the alpha to 1 to show the overlay again
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);
        isDamageOverlayActive = true;
        durationTimer = 0; // Reset timer to start fading
    }

    // Function to show heal overlay
    void ShowHealOverlay()
    {
        // Always reset the alpha to 1 to show the overlay again
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 1);
        isHealOverlayActive = true;
        durationTimer = 0; // Reset timer to start fading
    }

    public void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString() + "/100";
        }
    }
}
