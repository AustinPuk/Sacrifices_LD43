using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : MonoBehaviour
{
    // Cooldown Parameters
    bool canGrab = true;
    bool playerInside = false;

    [SerializeField]
    float timer = 0.1f;

    void Update()
    {
        if (playerInside && canGrab)
        {
            Game.game.minionPool.Gather();
            canGrab = false;
            StartCoroutine(Cooldown());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(timer); // TEMP
        canGrab = true;
    }
}
