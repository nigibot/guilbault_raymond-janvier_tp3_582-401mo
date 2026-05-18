using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class joueur : MonoBehaviour
{

    [SerializeField] private InputActionProperty jumpButton;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private CharacterController chat;
    [SerializeField] private LayerMask groundLayer;

    private float gravity = Physics.gravity.y;
    private Vector3 mouvement;

    public int mouseCounter = 20;
    public int Compteur = 8;

    private void Start()
    {
        InvokeRepeating("Heure", 1f, 30f);
    }

    private void Update()
    {
        bool _isGrounded = isGrounded(); 

        if(jumpButton.action.WasPressedThisFrame()&& _isGrounded)
        {
            Jump();
        }

        mouvement.y += gravity * Time.deltaTime;

        chat.Move(mouvement * Time.deltaTime);

        if (mouseCounter == 0) {
            Victoire();
            mouseCounter = -1;
        }

        if (Compteur == 0) {
            Defaite();
            Compteur = -1;
        }
    }

    private void Heure()
    {
        if (SceneManager.GetActiveScene().name == "jeu") {
            Compteur--;
        }
    }

    private void Jump()
    {
        mouvement.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity); 
    }

    private bool isGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.2f, groundLayer); 
    }

    private void Victoire()
    {
        Debug.Log("Victoire !");
        SceneManager.LoadScene("victoire");
    }

    private void Defaite()
    {
        Debug.Log("Défaite !");
        SceneManager.LoadScene("defaite");
    }
}