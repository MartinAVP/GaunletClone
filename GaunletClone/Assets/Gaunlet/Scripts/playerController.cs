using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;

    private Vector2 moveInput;
    public GameObject playerModel;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector3(moveInput.x, 0f, moveInput.y));
    }

    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (context.performed)
        {
            changeDirection();
        }

        //print(moveInput.x);
    }

    public void changeDirection()
    {
        // North East
        if(moveInput.y >= 0.5f && moveInput.x >= 0.5f)
        {
            playerModel.transform.rotation = Quaternion.Euler(0, 45, 0);
        }
        // South East
        else if(moveInput.y <= -0.5f && moveInput.x >= 0.5f)
        {
            playerModel.transform.rotation = Quaternion.Euler(0, 135, 0);
        }
        // South West
        else if(moveInput.y <= -0.5f && moveInput.x <= -0.5f)
        {
            playerModel.transform.rotation = Quaternion.Euler(0, -135, 0);
        }
        // North West
        else if (moveInput.y >= 0.5f && moveInput.x <= -0.5f)
        {
            playerModel.transform.rotation = Quaternion.Euler(0, -45, 0);
        }

        // Check forward and Backward
        else if(moveInput.y >= 0.5f)
        {
                    playerModel.transform.rotation = new Quaternion(0, 0, 0, 0);
                    //Debug.Log("Check Dir Front");
        }
        else if (moveInput.y <= -0.5f)
        {
                    //Debug.Log("Check Dir Back");
                    playerModel.transform.rotation = new Quaternion(0, 180, 0, 0);
        }

        // Check Right and Left
        else if(moveInput.x >= 0.5f)
        {
            //playerModel.transform.rotation = new Quaternion(0, 90, 0, 0);
            playerModel.transform.rotation = Quaternion.Euler(0, 90, 0);
            //Debug.Log("Check Dir Right");
        }
        else if (moveInput.x <= -0.5f)
        {
            //Debug.Log("Check Dir Left");
            playerModel.transform.rotation = Quaternion.Euler(0, -90, 0);
            //playerModel.transform.rotation = new Quaternion(0, -90, 0, 0);
        }
    }
}
