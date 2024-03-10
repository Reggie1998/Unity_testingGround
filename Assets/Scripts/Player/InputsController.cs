using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ThirdPersonController))]
public class InputsController : MonoBehaviour
{
    MovementHandler movementHandler;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMove(InputValue value)
    {
        var movementInput = value.Get<Vector2>();

        //movementHandler.OnMovementPressed(movementInput);
        //Debug.Log(value.Get<Vector2>());
    }

    public void Jump()
    {
        Debug.Log("Jump");
    }

    public void OnJump()
    {
        Debug.Log("OnJump");
    }
}

public class MovementHandler
{
    private Vector3 movementVector;
    private bool inDash = false;
    private Vector2 dashVelocity;
    private float verticalVelocity;
    private bool trackedIsGrounded;
    private float movementSpeed = 10.0f;
    private Vector2 lastMovementInputVelocity;
    private bool movementInput;
    public CharacterController charController;


    public Vector2 movementInputVelocity;

    public void HandleMovement()
    {
        movementVector = new Vector3(0.0f, verticalVelocity, 0.0f);
        if (inDash) {
            movementVector.x = dashVelocity.x;
            movementVector.z = dashVelocity.y;
        } else if (!trackedIsGrounded) {
            var lateralMovement = new Vector2(movementInputVelocity.x * movementSpeed, movementInputVelocity.y * movementSpeed);
            if (lastMovementInputVelocity != Vector2.zero) {
                var lerpedMovement = Vector2.Lerp(lastMovementInputVelocity, lateralMovement, 0.8f);
                movementVector = new Vector3(lerpedMovement.x, verticalVelocity, lerpedMovement.y);
            } else {
                movementVector.x = lateralMovement.x;
                movementVector.z = lateralMovement.y;
            }
        } else if (movementInput) {
            movementVector.x = movementInputVelocity.x * movementSpeed;
            movementVector.z = movementInputVelocity.y * movementSpeed;
        }

        charController.Move(movementVector * Time.deltaTime);
    }

    public void OnMovementPressed(Vector2 inputVal)
    {
        movementInput = true;
        movementInputVelocity = inputVal.normalized;
        //lastMovementInputVelocity = Vector2.zero;
    }
}
