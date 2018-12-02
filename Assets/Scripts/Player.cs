using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 5;

    [SerializeField]
    float shootDelay = 0.5f;

    [SerializeField]
    float maxHealth = 10.0f;

    [SerializeField]
    float knockbackForce;

    [SerializeField]
    float knockbackTime;

    [SerializeField]
    float invincibleTime;

    float currentHealth;
    bool invincible;

    Vector3 movementVector;
    Rigidbody rb;
    Animator animator;

    enum PlayerState
    {
        Inactive,
        Active,
        Shooting,
        Stunned
    }

    PlayerState state;

    public float HealthPercent { get { return currentHealth / maxHealth; } }

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

    public void Damage(Vector3 origin)
    {
        if (invincible)
            return;

        Debug.Log("Player Damaged");

        currentHealth -= 1.0f; // TODO

        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            Die();
        }
        else
        {
            Vector3 dir = transform.position - origin;;
            StartCoroutine(Knockback(dir.normalized));
            invincible = true;
            StartCoroutine(Invincibility());
        }
    }

    public void Restart()
    {
        state = PlayerState.Active;
        currentHealth = maxHealth;
    }

    /*
     *  Main Functions
     */
    void Awake ()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;

        //Temp. Should be activated by Game Manager
        state = PlayerState.Active;
    }

    void Update ()
    {
        ProcessInput();
        HandleMovement();
    }

    IEnumerator Knockback(Vector3 direction)
    {
        state = PlayerState.Stunned;
        rb.velocity = direction * knockbackForce;
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
        state = PlayerState.Active;
    }

    IEnumerator Invincibility()
    {
        Coroutine flash = StartCoroutine(FlashRed());
        yield return new WaitForSeconds(invincibleTime);
        StopCoroutine(flash);
        invincible = false;

        Color orig = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.material.color = orig;
    }

    IEnumerator FlashRed()
    {
        Color orig = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        Color red = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        while (true)
        {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
                rend.material.color = red;

            yield return new WaitForSeconds(0.1f);

            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
                rend.material.color = orig;

            yield return new WaitForSeconds(0.1f);
        }
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
            animator.SetFloat("Speed", Vector3.Magnitude(rb.velocity));

            // Character tries to face in direction of movement vector.
            if (movementVector != Vector3.zero)
                transform.LookAt(transform.position + Vector3.Slerp(transform.forward, movementVector, 0.8f), transform.up);
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }

    void Die()
    {
        // TODO : Death Animation

        Game.game.PlayerDies();
    }

    IEnumerator ShootAnimation()
    {
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootDelay);
        animator.SetTrigger("ShootEnd");
        state = PlayerState.Active;
    }
}
