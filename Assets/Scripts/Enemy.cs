using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 5;

    [SerializeField]
    float attackDistance;

    Rigidbody rb;
    Collider collider;
    GameObject player;

    enum EnemyState
    {
        Inactive,
        Follow,
        Attack,
        Cooldown,
        Damage, // Being Damage
        Dead
    }

    EnemyState state;

    public bool IsInactive { get { return state == EnemyState.Inactive || state == EnemyState.Dead; } }

    Coroutine attacking;

    /*
     *  Public Functions
     */

    public void Spawn(Vector3 spawnLocation)
    {
        state = EnemyState.Follow;
        transform.position = spawnLocation;
        if (collider)
            collider.enabled = true;
        // Moves enemy to spawn location
    }

    public void Damage()
    {
        Debug.Log("Enemy takes Damage");
        StartCoroutine(TakeDamage());
    }

    /*
     *  Main Functions
     */
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //Temp
        state = EnemyState.Inactive;
        movementSpeed *= Random.Range(0.8f, 1.1f);
    }

    void Update()
    {
        if (!player) // TODO : Better
            player = Game.game.player.gameObject;

        HandleMovement();
    }

    void OnDrawGizmos()
    {
        // DEBUG
        if (state == EnemyState.Attack)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
        // DEBUG
        else if (state == EnemyState.Follow)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }


        void HandleMovement()
    {
        if (state == EnemyState.Follow)
        {
            /*
             * TODO : 
             *  Add small randomness to movement speed for each enemy to prevent overlap
             *  Should always be facing player, except when they get hit
             *  Prevent them from going on top of eachother. Fix physics a bit
             */

            // Go to area behind player
            Vector3 destination = player.transform.position;
            Vector3 movementVector = (destination - transform.position).normalized;

            float dist = Vector3.SqrMagnitude(destination - transform.position);
            if (dist >= (attackDistance * attackDistance))
            {
                rb.velocity = movementVector * movementSpeed;

                // Tries to face in direction of movement vector.
                if (movementVector != Vector3.zero)
                    transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
            }
            else
                attacking = StartCoroutine(Attack());
        }
        else
            rb.velocity = Vector3.zero;
    }

    void Dead()
    {
        //TODO
        transform.position = Vector3.down * 10.0f;
    }

    IEnumerator Attack()
    {
        rb.velocity = Vector3.zero;
        transform.LookAt(player.transform.position, transform.up);
        state = EnemyState.Attack;
        // TODO : Attacks should have a few frames of delay before the attack, sync with animation

        Debug.Log("Enemy : Attacking");

        yield return new WaitForSeconds(0.2f);

        state = EnemyState.Follow;
        // TODO 
    }

    IEnumerator TakeDamage()
    {

        // Stop attacking and stop any animations
        if (state == EnemyState.Attack)
            StopCoroutine(attacking);

        // Stop and make enemy lose collider
        rb.velocity = Vector3.zero;

        // Take Damage
        state = EnemyState.Damage;

        // Play Death Animation

        yield return new WaitForSeconds(0.1f);

        // Particle Effects and Knockback

        state = EnemyState.Dead;
        Dead();
    }
}
