using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.WeaponActions weaponActions;
    public PlayerInput.ItemActions itemActions;
    private PlayerMotor motor;
    private PlayerLook look;
    private WeaponManager weaponManager;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        weaponActions = playerInput.Weapon;
        itemActions = playerInput.Item;

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
        itemActions.Enable();

        // Jump
        onFoot.Jump.performed += ctx => motor.Jump();

        // Shoot
        weaponActions.Shoot.started += ctx => weaponManager.HandleFire(true);  // Start firing
        weaponActions.Shoot.canceled += ctx => weaponManager.HandleFire(false);  // Stop firing

        // Reload
        weaponActions.Reload.performed += ctx => weaponManager.Reload(); // Reload

        // Weapon switch
        weaponActions.SwitchWeapon.performed += ctx => OnSwitchWeapon(ctx);  // Switch weapons

        // ADS (Aim Down Sights) action
        weaponActions.ADS.performed += ctx => weaponManager.handleADS();  // Toggle ADS on right click/left trigger

        // Throwable
        itemActions.Throwable.performed += ctx => weaponManager.handleThrowable();

        // Tactical
        itemActions.Tactical.performed += ctx => weaponManager.handleTactical();
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
        itemActions.Disable();
    }
}
