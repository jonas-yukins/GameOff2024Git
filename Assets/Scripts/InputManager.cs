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
    private Weapon weapon;

    void Awake() {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        weaponActions = playerInput.Weapon;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        weapon = GetComponent<Weapon>();

        onFoot.Jump.performed += ctx => motor.Jump();

        weaponActions.Shoot.performed += ctx => weapon.StartShot();
    }

    // Update is called once per frame
    void FixedUpdate() {
        //tell the playermotor to move using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate() { 
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable() {
        onFoot.Enable();
        weaponActions.Enable();
    }

    private void OnDisable() {
        onFoot.Disable();
        weaponActions.Disable();
    }
}
