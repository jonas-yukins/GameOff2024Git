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
    private WeaponSway weaponSway; // Reference to WeaponSway

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        weaponActions = playerInput.Weapon;
        itemActions = playerInput.Item;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        weaponManager = GetComponent<WeaponManager>();
        weaponSway = GetComponentInChildren<WeaponSway>();

        // Set the initial inputs to WeaponSway
        weaponSway.walkInput = Vector2.zero;
        weaponSway.lookInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Pass movement input to PlayerMotor (for movement)
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

        // Update WeaponSway's input
        if (weaponSway != null)
        {
            weaponSway.walkInput = onFoot.Movement.ReadValue<Vector2>();
        }
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        
        // Update WeaponSway's look input
        if (weaponSway != null)
        {
            weaponSway.lookInput = onFoot.Look.ReadValue<Vector2>();
        }
    }

    private void OnEnable()
    {
        onFoot.Enable();
        weaponActions.Enable();
        itemActions.Enable();

        onFoot.Jump.performed += ctx => motor.Jump();
        weaponActions.Shoot.started += ctx => weaponManager.HandleFire(true);
        weaponActions.Shoot.canceled += ctx => weaponManager.HandleFire(false);
        weaponActions.Reload.performed += ctx => weaponManager.Reload();
        weaponActions.SwitchWeapon.performed += ctx => OnSwitchWeapon(ctx);
        weaponActions.ADS.performed += ctx => OnADS(ctx);
        itemActions.Throwable.performed += ctx => weaponManager.handleThrowable();
        itemActions.Tactical.performed += ctx => weaponManager.handleTactical();
    }

    private void OnSwitchWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (ctx.control.name == "1") weaponManager.SwitchWeapon(0);
            else if (ctx.control.name == "2") weaponManager.SwitchWeapon(1);
            else if (ctx.control.name == "3") weaponManager.SwitchWeapon(2);
        }
    }

    private void OnADS(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            weaponSway.isADS = !weaponSway.isADS;  // Toggle ADS state
            weaponManager.handleADS();  // Call the toggleADS method in Weapon
        }
    }

    private void OnDisable()
    {
        onFoot.Disable();
        weaponActions.Disable();
        itemActions.Disable();
    }
}
