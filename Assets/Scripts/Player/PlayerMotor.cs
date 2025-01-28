using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    //private Animator animator; 
    // commented out animations for player capsule
    private Vector3 playerVelocity;
    public bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    // This value is the maximum speed the player can fall at
    public float terminalVelocity = -53f;  // Cap the fall speed to prevent extreme floating

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        isGrounded = controller.isGrounded;
        // Update the IsGrounded parameter in the Animator
        //animator.SetBool("isGrounded", isGrounded);
    }
    
    //receive the inputs for our InputManager.cs and apply them to our character controller.
    public void ProcessMove(Vector2 input) {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        // Apply gravity
        if (!isGrounded) {
            playerVelocity.y += gravity * Time.deltaTime;  // Fall normally if not grounded
        } else if (playerVelocity.y < 0) {
            playerVelocity.y = -2f;  // This value prevents the player from "sticking" to the ground
        }

        // Apply movement due to gravity (falling speed capped to terminal velocity)
        playerVelocity.y = Mathf.Max(playerVelocity.y, terminalVelocity);  // Cap the fall speed

        // Apply velocity to character controller
        controller.Move(playerVelocity * Time.deltaTime);

        // Calculate the movement speed and update the Animator
        float movementSpeed = new Vector2(input.x, input.y).magnitude;
        //animator.SetFloat("Speed", movementSpeed);  // Update the Speed parameter

        // If the player is on the ground, make sure jump animation transitions out
        //if (isGrounded && playerVelocity.y <= 0f) {
        //    animator.ResetTrigger("Jump");
        //}
    }

    public void Jump() {
        if (isGrounded) {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

            // Trigger jump animation
            //animator.SetTrigger("Jump");
        }
    }
}
