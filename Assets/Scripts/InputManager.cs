using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.WeaponActions weaponActions;
    private PlayerMotor motor;
    private PlayerLook look;
    private WeaponManager weaponManager; // Reference to WeaponManager to control weapons

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        weaponActions = playerInput.Weapon;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        weaponManager = GetComponent<WeaponManager>();  // Get the WeaponManager component
    }

    void FixedUpdate()
    {
        motor.ProcessMove (onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    { 
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
        weaponActions.Enable();  // Enable weapon input actions for firing

        // Bind input actions to methods
        onFoot.Jump.performed += ctx => motor.Jump();
        weaponActions.Shoot.started += ctx => weaponManager.StartFire();
        weaponActions.Shoot.canceled += ctx => weaponManager.StopFire();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        weaponActions.Disable();  // Disable weapon input actions
    }

    // Handle weapon firing through the input system
    public void OnShootStarted(InputAction.CallbackContext context)
    {
        weaponManager.currentWeapon?.StartFire();
    }

    public void OnShootCanceled(InputAction.CallbackContext context)
    {
        weaponManager.currentWeapon?.StopFire();
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.WeaponActions weaponActions;
    private PlayerMotor motor;
    private PlayerLook look;
    private WeaponManager weaponManager; // Reference to WeaponManager to control weapons

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        weaponActions = playerInput.Weapon;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        weaponManager = GetComponent<WeaponManager>();  // Get the WeaponManager component
    }

    void OnEnable()
    {
        // Enable input actions when the game starts
        onFoot.Enable();
        weaponActions.Enable();
        
        // Bind input actions to methods
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Movement.performed += ctx => motor.ProcessMove(ctx.ReadValue<Vector2>());
        onFoot.Look.performed += ctx => look.ProcessLook(ctx.ReadValue<Vector2>());
        
        weaponActions.Shoot.started += ctx => weaponManager.StartFire();
        weaponActions.Shoot.canceled += ctx => weaponManager.StopFire();
    }

    void OnDisable()
    {
        // Disable input actions when the player is disabled
        onFoot.Disable();
        weaponActions.Disable();
    }
}

*/