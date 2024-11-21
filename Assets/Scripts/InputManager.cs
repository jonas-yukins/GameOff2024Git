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
        weaponActions.Reload.performed += ctx => weaponManager.Reload();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        weaponActions.Disable();
    }
}
