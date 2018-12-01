using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //Sphere Collider

    //other explosion parameters here

    bool isExploding = false;

    public void Boom()
    {
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        // Particle Effect Stuff

        // Expanding Sphere Collider
    }

    private void Update()
    {
        if (isExploding)
        {
            // Expand Sphere Collider over time
        }

    }

    // For detecting when the expanding sphere intersects with an enemy or player
    void OnTriggerEnter(Collider other)
    {
        // If other.gameObject is of type Damageable, do damage
    }
}