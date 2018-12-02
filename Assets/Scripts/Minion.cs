using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 5;

    [SerializeField]
    float followDistance;

    [SerializeField]
    float stopDistance;

    [SerializeField]
    float speedBoost = 3.0f;

    Vector3 movementVector;
    Rigidbody rb;
    Animator animator;
    Vector3 shootDest;
    Player king;

    [SerializeField]
    Explosion explosion; // TODO :Better way to pass object

    [SerializeField]
    ParticleSystem particleShootReady;

    [SerializeField]
    ParticleSystem particleShootTrail;

    [SerializeField]
    ParticleSystem particleSelfDestruct;

    [SerializeField]
    int selfDestructCount; // Self destructs if collides with enemy too many times (surrounded)

    public bool IsInactive { get { return state == MinionState.Inactive; } }

    enum MinionState
    {
        Inactive,
        Follow,
        ShootPrep, // Pre-Shoot Animation (spin)
        ShootMove, // Move to Shoot Destination
        ShootReady, // Signal Pre-Shoot Animation (Build Up)
        Shoot,      // Is being Shot, will explode on collision
        ShootFinish, // Post-shoot logic
        Exploding,
        Dead   // ?
    }

    MinionState state = MinionState.Inactive;

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
        // TEMP Set as trigger to avoid collisions. In the future, want to have the minion properly walk around the player.
        GetComponent<Collider>().isTrigger = true;

        shootDest = king.transform.position + (shootDirection * followDistance);
        StartCoroutine(ShootPrep());
    }

    public void Damage()
    {
        // TODO : Death Animation or Effect
        if (state == MinionState.Follow)
            StartCoroutine(DelayedBoom());
    }

    /*
     *  Main Minion Commands
     */

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!king)
            king = Game.game.player;

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
            Vector3 movementVector = destination - transform.position;

            float distToKing = Vector3.SqrMagnitude(king.transform.position - transform.position);
            float distToDest = Vector3.SqrMagnitude(destination - transform.position);
            float stopDistanceSqr = stopDistance * stopDistance;
            if (distToKing < stopDistanceSqr || distToDest < stopDistanceSqr)
                rb.velocity = movementVector * movementSpeed; // Slowed down for close distances
            else
                rb.velocity = movementVector.normalized * movementSpeed; // Normalized for far distances

            // Minions always face player
            transform.LookAt(king.transform.position, transform.up);
        }
        else if (state == MinionState.ShootMove)
        {
            // Should speed to destination in front of player
            Vector3 movementVector = shootDest - transform.position;

            float destThreshold = 0.3f; // temp
            if (Vector3.SqrMagnitude(shootDest - transform.position) > destThreshold * destThreshold)
            {
                rb.velocity = movementVector.normalized * movementSpeed;
            }
            else // Arrived to destination, Wait for ShootReady
            {
                // Snap to proper location
                rb.velocity = movementVector * movementSpeed;
            }
        }
        else if (state == MinionState.ShootReady)
        {
            // Snap to proper location
            transform.position = shootDest;

            // Look in proper direction
            rb.velocity = Vector3.zero;
            transform.LookAt(king.transform.position + (shootDest - king.transform.position) * 5.0f, transform.up);

            ShootAnimation();
        }
        else if (state == MinionState.Shoot)
        {
            Vector3 movementVector = transform.forward;
            GetComponent<Rigidbody>().velocity = movementVector * movementSpeed * speedBoost;
        }

        //TODO : Logic for when coming out of castle
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state == MinionState.Follow && collision.gameObject.tag == "Enemy")
        {
            // Self Destructs on touching enemy
            selfDestructCount--;

            if (selfDestructCount <= 0)
                StartCoroutine(DelayedBoom());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (state != MinionState.Shoot && state != MinionState.ShootReady)
            return;

        if (other.tag == "Environment" || other.tag == "Enemy")
        {
            Boom();
        }
    }

    void Boom()
    {

        GetComponent<Collider>().enabled = false;

        state = MinionState.Dead; // TEMP
        rb.velocity = Vector3.zero;
        explosion.Boom();
        StartCoroutine(Die());

        // Initiate explosion sequence

        // Enable child explosion object

        // For now, cause this object to fade and disappear back into the pool.


        // BONUS : Make minions stay dead on battlefield
    }

    IEnumerator ShootPrep()
    {
        // Play and wait for Build Up Animation (should be quick)
        rb.velocity = Vector3.zero;
        transform.LookAt(king.transform.position + (shootDest - king.transform.position) * 10.0f, transform.up);
        state = MinionState.ShootPrep;
        animator.SetTrigger("Spin");
        yield return new WaitForSeconds(0.2f);

        // Move to position
        state = MinionState.ShootMove;

        // TODO :Properly time end of animation
        yield return new WaitForSeconds(0.4f);

        // Minion ready shoot
        state = MinionState.ShootReady;
    }

    void ShootAnimation()
    {
        animator.SetTrigger("Shoot");

        particleShootReady.Play();
        particleShootTrail.Play();

        state = MinionState.Shoot;

        // Check if already on top of a colliding object
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.3f); // Hardcoded rn
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.tag == "Environment" || hit.gameObject.tag == "Enemy")
            {
                Boom();
                break;
            }
        }
    }

    IEnumerator Die()
    {
        // TODO
        Game.game.MinionDies();
        particleShootReady.Stop();
        particleShootTrail.Stop();
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(0.5f);
        //transform.position = Vector3.down * 10.0f;
        Destroy(gameObject); // Temporarily going with this for now
    }

    IEnumerator DelayedBoom()
    {
        Game.game.minionPool.ForcedRemove(this);
        state = MinionState.Exploding;
        animator.SetTrigger("Spin");
        particleSelfDestruct.Play();
        yield return new WaitForSeconds(0.3f);
        Boom();
    }
}
