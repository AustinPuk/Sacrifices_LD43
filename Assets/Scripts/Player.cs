﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 5;

    Vector3 movementVector;
    Rigidbody rb;

    enum PlayerState
    {
        Inactive,
        Active,
        Shooting
    }

    PlayerState state;

    /*
     *  Public Functions
     */

    public void Shoot(Vector3 shootDirection)
    {
        state = PlayerState.Shooting;

        rb.velocity = Vector3.zero;
        transform.LookAt(transform.position + shootDirection, transform.up);

        StartCoroutine(ShootAnimation());
    }

    /*
     *  Main Functions
     */
    void Start ()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        //Temp
        state = PlayerState.Active;
    }

    void Update ()
    {
        ProcessInput();
        HandleMovement();
    }

    void ProcessInput()
    {
        // Gets Movement Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        movementVector = new Vector3(h, 0, v);
    }

    void HandleMovement()
    {
        if(state == PlayerState.Active)
        {
            // Utilizing Character Controller for Movement
            //controller.Move(movementVector * movementSpeed * Time.deltaTime);
            rb.velocity = movementVector * movementSpeed;

            // Character tries to face in direction of movement vector.
            if (movementVector != Vector3.zero)
                transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
        }
    }

    IEnumerator ShootAnimation()
    {
        Debug.Log("Player is Shooting");
        yield return new WaitForSeconds(0.5f); // TEMP
        state = PlayerState.Active;
    }
}
