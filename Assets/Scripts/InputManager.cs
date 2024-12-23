using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.WeaponActions weaponActions;
    private PlayerMotor motor;
    private PlayerLook look;
    private WeaponManager weaponManager;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        weaponActions = playerInput.Weapon;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        weaponManager = GetComponent<WeaponManager>();
    }

    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        // Enable actions
        onFoot.Enable();
        weaponActions.Enable();

        // Bind actions to methods
        onFoot.Jump.performed += ctx => motor.Jump();
        weaponActions.Shoot.started += ctx => weaponManager.HandleFire(true);  // Start firing
        weaponActions.Shoot.canceled += ctx => weaponManager.HandleFire(false);  // Stop firing
        weaponActions.Reload.performed += ctx => weaponManager.Reload(); // Reload

        // Add weapon switching
        weaponActions.SwitchWeapon.performed += ctx => OnSwitchWeapon(ctx);  // Switch weapons

        // Add ADS (Aim Down Sights) action
        weaponActions.ADS.performed += ctx => weaponManager.handleADS();  // Toggle ADS on right click/left trigger
    }

    private void OnSwitchWeapon(InputAction.CallbackContext ctx)
    {
        // Use the ctx.action phase to ensure that we are handling a button press (started or performed).
        if (ctx.performed)  // Only handle the switch when the action is performed (key pressed)
        {
            // Check which key was pressed and switch the weapon accordingly.
            if (ctx.action.name == "SwitchWeapon")
            {
                if (ctx.control.name == "1")
                {
                    // Switch to the first weapon (Pistol)
                    weaponManager.SwitchWeapon(0);
                    Debug.Log("Switched to Pistol");
                }
                else if (ctx.control.name == "2")
                {
                    // Switch to the second weapon (Shotgun)
                    weaponManager.SwitchWeapon(1);
                    Debug.Log("Switched to Assault Rifle");
                }
                else if (ctx.control.name == "3")
                {
                    // Switch to the third weapon (Assault Rifle)
                    weaponManager.SwitchWeapon(2);
                    Debug.Log("Switched to Shotgun");
                }
            }
        }
    }

    private void OnDisable()
    {
        onFoot.Disable();
        weaponActions.Disable();
    }
}
