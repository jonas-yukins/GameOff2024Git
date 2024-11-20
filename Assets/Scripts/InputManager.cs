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
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
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
        weaponActions.Reload.performed += ctx => weaponManager.Reload();
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