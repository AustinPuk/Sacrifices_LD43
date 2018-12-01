using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //Sphere Collider

    //other explosion parameters here
    ParticleSystem particles;

    bool isExploding = false;

    public void Boom()
    {
        particles.Play();

        // TODO Make Sphere params parameters
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.GetComponent<Enemy>() != null)
            {
                hit.gameObject.GetComponent<Enemy>().Damage();
            }
            else if (hit.gameObject.GetComponent<Player>() != null)
            {
                hit.gameObject.GetComponent<Player>().Damage();
            }
        }
    }

    void OnDrawGizmos()
    {
        // DEBUG
        if (particles && particles.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 3.0f);
        }
    }


    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (isExploding)
        {
            //TODO
            // Expand Sphere Collider over time
        }

    }
}