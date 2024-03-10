using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Rigidbody rb;

    public float rotationSpeed;

    public GameObject thirdPersonCam;
    public GameObject combatCam;

    public CameraStyle currentStyle;

    public float speed;

    public Vector2 inputVector;
    public enum CameraStyle
    {
        Basic,
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RotateTowardsCamera() {
        // Get the direction towards the camera
        Vector3 directionToCamera = Camera.main.transform.forward;

        // Ignore the camera's Y axis rotation
        directionToCamera.y = 0;

        // Rotate the object to face the camera's direction
        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private bool IsGrounded () {
        //Simple way to check for ground
        if (Physics.Raycast (transform.position, Vector3.down, 1.5f)) {
            return true;
        } else {
            return false;
        }
    }

    void HandleMovementInput() {
        // Get input axis values
        float horizontalInput = this.inputVector.x * speed; 
        float verticalInput = this.inputVector.y * speed;

        // Calculate movement direction based on input
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotate movement direction according to the camera's forward direction
        Quaternion camRotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
        movementDirection = camRotation * movementDirection;
        rb.velocity = movementDirection * speed;
    }

    void FixedUpdate()
    {
        RotateTowardsCamera();
        HandleMovementInput();
    }
}
