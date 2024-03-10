using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ThirdPersonController))]
public class InputsController : MonoBehaviour
{

    private ThirdPersonController tpsc;

    // Start is called before the first frame update
    void Start()
    {
        tpsc = GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMove(InputValue value)
    {
        tpsc.inputVector = value.Get<Vector2>();
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