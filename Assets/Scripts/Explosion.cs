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
        //gameObject.SetActive(true);

        particles.Play();

        // TODO Make Sphere params parameters
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Player")
            {
                //Do Damage
                Debug.Log("Doing Damage to : " + hit.name);
            }
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