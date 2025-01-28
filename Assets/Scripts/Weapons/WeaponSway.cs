using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private PlayerMotor playerMotor;
    private Vector3 initialPosition; // The weapon's initial position relative to the camera

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    public float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    [Header("ADS Sway")]
    public float adsSwayMultiplier = 0.2f;  // Lower values make the weapon less swayed
    public bool isADS = false;  // State for whether ADS is active

    // Public variables for walk and look input
    public Vector2 walkInput;
    public Vector2 lookInput;

    void Awake()
    {
        playerMotor = GetComponentInParent<PlayerMotor>();
        initialPosition = transform.localPosition; // Store the initial position of the weapon relative to the camera
    }

    void Update()
    {
        // Update the sway and bobbing
        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }

    void Sway()
    {
        // Apply ADS sway reduction
        float swayFactor = isADS ? adsSwayMultiplier : 1f;  // Use a multiplier for ADS sway reduction

        Vector3 invertLook = lookInput * -step * swayFactor;  // Scale sway by factor
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }


    void SwayRotation()
    {
        // Apply ADS sway reduction
        float rotationFactor = isADS ? adsSwayMultiplier : 1f;  // Use a multiplier for ADS sway rotation

        Vector2 invertLook = lookInput * -rotationStep * rotationFactor;  // Scale rotation by factor
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }


    void CompositePositionRotation()
    {
        // Apply the sway as an offset from the initial position in front of the camera
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + swayPos + bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void BobOffset()
    {
        // Apply a reduced bobbing factor if ADS is active, and only bob when the player is moving
        float bobbingFactor = isADS ? 0f : 1f;  // Disable bobbing during ADS

        // Only apply bobbing if there's movement input
        if (walkInput.magnitude > 0.1f && playerMotor.isGrounded)  // Check if there's any significant movement input
        {
            speedCurve += Time.deltaTime * (playerMotor.isGrounded ? (walkInput.x + walkInput.y) * bobExaggeration * bobbingFactor : 1f) + 0.01f;

            bobPosition.x = (curveCos * bobLimit.x * (playerMotor.isGrounded ? 1 : 0)) - (walkInput.x * travelLimit.x);
            bobPosition.y = (curveSin * bobLimit.y) - (walkInput.y * travelLimit.y);
            bobPosition.z = -(walkInput.y * travelLimit.z);
        }
        else
        {
            // If there's no movement, set the bob position to zero to stop the bobbing effect
            bobPosition = Vector3.zero;
        }
    }


    void BobRotation()
    {
        // Apply a reduced rotation factor if ADS is active, and only apply rotation bobbing when moving
        float rotationFactor = isADS ? 0f : 1f;  // Disable rotation during ADS

        // Only apply rotation if there's movement input
        if (walkInput.magnitude > 0.1f && playerMotor.isGrounded)  // Check if there's any significant movement input
        {
            bobEulerRotation.x = walkInput != Vector2.zero ? multiplier.x * Mathf.Sin(2 * speedCurve) * rotationFactor : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2);
            bobEulerRotation.y = walkInput != Vector2.zero ? multiplier.y * curveCos * rotationFactor : 0;
            bobEulerRotation.z = walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x * rotationFactor : 0;
        }
        else
        {
            // If there's no movement, set the rotation to zero to stop the rotation effect
            bobEulerRotation = Vector3.zero;
        }
    }


}
