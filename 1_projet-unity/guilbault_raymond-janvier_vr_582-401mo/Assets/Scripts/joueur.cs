using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class joueur : MonoBehaviour
{
    [SerializeField] private InputActionProperty jumpButton;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private CharacterController chat;
    [SerializeField] private LayerMask groundLayer;

    private float gravity = Physics.gravity.y;
    private Vector3 mouvement;

    private void Update()
    {
        bool _isGrounded = isGrounded(); 

        if(jumpButton.action.WasPressedThisFrame()&& _isGrounded)
        {
            Jump();
        }

        mouvement.y += gravity * Time.deltaTime;

        chat.Move(mouvement * Time.deltaTime);
    }


    private void Jump()
    {
        mouvement.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity); 
    }

    private bool isGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.2f, groundLayer); 
    }
}