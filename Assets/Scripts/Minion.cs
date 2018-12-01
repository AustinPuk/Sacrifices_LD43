using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    //NavMeshAgent nav;

    Rigidbody rb;

    Vector3 shootDest;

    enum MinionState
    {
        Hidden,
        Idle,
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
        state = MinionState.Follow;
    }

    public void Shoot(Vector3 shootDirection)
    {
        Debug.Log("Minion : Shoot Prep");

        shootDest = king.transform.position + (shootDirection * followDistance);
        state = MinionState.ShootPrep;

        // Note : Shoot Direction is relative to Player

        // Move to in front of player in the proper shooting direction (not side, cause that'd mess with direction)

        // Pause, Animation (Probably Coroutine)

        // Shoot out forward

        // Enable "isShooting" bool
    }

    /*
     *  Main Minion Commands
     */

    void Start()
    {
        //nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        // TEMP
        Follow();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

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
            Vector3 movementVector = destination - transform.position;

            // Utilizing Character Controller for Movement
            //controller.Move(movementVector * movementSpeed * Time.deltaTime);

            float dist = Vector3.SqrMagnitude(king.transform.position - transform.position);
            if (dist >= stopDistance)
                rb.velocity = movementVector * movementSpeed;
            else
                rb.velocity = Vector3.zero;

            //nav.SetDestination(destination);

            // Character tries to face in direction of movement vector.
            //if (movementVector != Vector3.zero)
                //transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
        }
        else if (state == MinionState.ShootPrep)
        {
            // Should speed to destination in front of player
            Vector3 movementVector = shootDest - transform.position;

            float destThreshold = 0.1f; // temp
            if (Vector3.SqrMagnitude(movementVector) > destThreshold)
            {
                // Utilizing Character Controller for Movement
                //controller.Move(movementVector * movementSpeed * speedBoost * Time.deltaTime);
                //nav.SetDestination(shootDest);
                rb.velocity = movementVector * movementSpeed * speedBoost;

                // Character tries to face in direction of movement vector.
                if (movementVector != Vector3.zero)
                    transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
            }
            else // Arrived to destination
            {
                // Look in proper direction
                //nav.isStopped = true;
                rb.velocity = Vector3.zero;
                transform.LookAt(shootDest + (shootDest - king.transform.position) * 5.0f, transform.up); // untested
                ShootPrepAnimation();
            }
        }
        else if (state == MinionState.Shoot)
        {
            Debug.Log("Minion : Shooting");
            Vector3 movementVector = transform.forward;
            GetComponent<Rigidbody>().velocity = movementVector * movementSpeed * speedBoost;
            //controller.Move(movementVector * movementSpeed * speedBoost * Time.deltaTime);
        }

        //TODO : Logic for when coming out of castle
    }

    // For detecting when a shot minion hits an object. May not be the right function.
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Minion is Colliding!");

        if (state != MinionState.Shoot)
            return;

        //TODO : When being shot, minions should ignore collisions with other minions. Should use the "trigger" instead of collide

        Boom();

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
        }
    }

    void Boom()
    {
        Debug.Log("Minion : BOOM HIT SOMETHING");

        state = MinionState.Idle; // TEMP

        // Initiate explosion sequence

        // Enable child explosion object

        // For now, cause this object to fade and disappear back into the pool.


        // BONUS : Make minions stay dead on battlefield
    }

    void ShootPrepAnimation()
    {
        Debug.Log("Minion : Prep Animation Ready");
        state = MinionState.ShootReady;
        // Play and wait for Build Up Animation (should be quick)

        // Wait for animation 

        // Play Particles plus shooting animation

        state = MinionState.Shoot;
    }
}
