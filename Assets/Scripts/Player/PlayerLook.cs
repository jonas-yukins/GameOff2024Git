using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;  // Reference to the Camera
    public Transform playerBody;  // Reference to the player's body (for rotating left/right)
    public Transform head;  // Reference to the player's head (or eye bone, where the camera is attached)
    private float xRotation = 0f;

    public float xSensitivity = 30f;  // Mouse sensitivity for looking left/right
    public float ySensitivity = 30f;  // Mouse sensitivity for looking up/down

    void Update()
    {
        // Get input for mouse movement
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        // Process camera rotation based on the mouse input
        ProcessLook(input);

        // Lock camera position relative to the head bone (to prevent animation bobbing)
        cam.transform.position = head.position;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Calculate camera rotation for looking up and down (vertical movement)
        xRotation -= mouseY * ySensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);  // Clamp vertical rotation to prevent extreme angles

        // Apply vertical rotation to the camera (up/down movement)
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player body (for left/right movement)
        playerBody.Rotate(Vector3.up * (mouseX * xSensitivity * Time.deltaTime));
    }
}
