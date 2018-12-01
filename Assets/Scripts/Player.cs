using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 5;

    Vector3 movementVector;

    void Start ()
    {
        // TODO : 
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
        // Character tries to face in direction of movement vector.
        transform.Translate(movementVector * movementSpeed * Time.deltaTime, Space.World);
        if (movementVector != Vector3.zero)
            transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
    }
}
