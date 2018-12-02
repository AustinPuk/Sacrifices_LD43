﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 5;

    [SerializeField]
    GameObject king; // TODO : Pass the player object from MinionPool instead.

    [SerializeField]
    float followDistance;

    [SerializeField]
    float stopDistance;

    [SerializeField]
    float speedBoost = 3.0f;

    Vector3 movementVector;

    Rigidbody rb;

    Vector3 shootDest;

    [SerializeField]
    Explosion explosion; // TODO :Better way to pass object

    public bool IsInactive { get { return state == MinionState.Inactive; } }

    enum MinionState
    {
        Inactive,
        Follow,
        ShootPrep, // Go to Front of Player (shootDest)
        ShootReady, // Signal Pre-Shoot Animation
        Shoot,      // Is being Shot, will explode on collision
        ShootFinish, // Post-shoot logic
        Dead   // ?
    }

    MinionState state;

    /*
    * Public Commands called from MinionPool
    */

    public void Follow()
    {
        GetComponent<Collider>().isTrigger = false;
        state = MinionState.Follow;
    }

    public void Shoot(Vector3 shootDirection)
    {
        Debug.Log("Minion : Shoot Prep");

        // TEMP Set as trigger to avoid collisions. In the future, want to have the minion properly walk around the player.
        GetComponent<Collider>().isTrigger = true;

        shootDest = king.transform.position + (shootDirection * followDistance);
        state = MinionState.ShootPrep;
    }

    /*
     *  Main Minion Commands
     */

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
        state = MinionState.Inactive;
        // TEMP
        //Follow();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // TODO : Consolidate movemtnt  better and reduce duplicate code
        if (state == MinionState.Follow)
        {
            /*
             * TODO : 
             *  Add randomness to movement speed for each minion for a bit more variety
             *  Should always be facing player, except when they get shot
             *  Prevent them from going on top of eachother. Fix physics a bit
             * 
             */

            // Go to area behind player
            Vector3 destination = king.transform.position + (king.transform.forward * -followDistance);
            Vector3 movementVector = (destination - transform.position).normalized;

            float dist = Vector3.SqrMagnitude(king.transform.position - transform.position);
            if (dist >= stopDistance)
                rb.velocity = movementVector * movementSpeed;
            else
                rb.velocity = Vector3.zero;

            // Character tries to face in direction of movement vector.
            if (movementVector != Vector3.zero)
                transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
        }
        else if (state == MinionState.ShootPrep)
        {
            // Should speed to destination in front of player
            Vector3 movementVector = (shootDest - transform.position).normalized;

            float destThreshold = 0.5f; // temp
            if (Vector3.SqrMagnitude(shootDest - transform.position) > destThreshold * destThreshold)
            {
                rb.velocity = movementVector * movementSpeed * speedBoost;

                // Character tries to face in direction of movement vector.
                if (movementVector != Vector3.zero)
                    transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
            }
            else // Arrived to destination
            {
                // Snap to proper location
                transform.position = shootDest;

                // Look in proper direction
                rb.velocity = Vector3.zero;
                transform.LookAt(transform.position + (transform.position - king.transform.position) * 5.0f, transform.up); // untested
                ShootPrepAnimation();
            }
        }
        else if (state == MinionState.Shoot)
        {
            Vector3 movementVector = transform.forward;
            GetComponent<Rigidbody>().velocity = movementVector * movementSpeed * speedBoost;
        }

        //TODO : Logic for when coming out of castle
    }

    void OnTriggerEnter(Collider other)
    {
        if (state != MinionState.Shoot && state != MinionState.ShootPrep)
            return;

        if (other.tag == "Environment" || other.tag == "Enemy")
        {
            Boom();
        }
    }

    void Boom()
    {

        state = MinionState.Dead; // TEMP
        rb.velocity = Vector3.zero;
        explosion.Boom();
        StartCoroutine(Die());

        // Initiate explosion sequence

        // Enable child explosion object

        // For now, cause this object to fade and disappear back into the pool.


        // BONUS : Make minions stay dead on battlefield
    }

    void ShootPrepAnimation()
    {
        state = MinionState.ShootReady;
        // Play and wait for Build Up Animation (should be quick)

        // Wait for animation 

        // Play Particles plus shooting animation

        state = MinionState.Shoot;
    }

    IEnumerator Die()
    {
        // TODO
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector3.down * 10.0f;
    }
}
