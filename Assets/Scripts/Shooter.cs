using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On mouse clicks, find direction to shoot and send signal to MinionPool & Player

public class Shooter : MonoBehaviour
{
    [SerializeField]
    Player player; // TODO : Probably have a "Game" class that pass these in instead of setting it this way

    [SerializeField]
    MinionPool minionPool; // TODO : ^

    // TODO : Cooldown Logic

    void Start ()
    {
    }

    void Update ()
    {
        if (!Game.game.isRunning)
            return;

        ProcessMouse();
    }

    void ProcessMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100.0f, 1 << LayerMask.NameToLayer("Ground"))) // Only hit ground layer
            {
                Vector3 shootDirection = Vector3.Normalize(hit.point - player.transform.position);
                shootDirection.y = 0.0f;
                Debug.DrawRay(player.transform.position, shootDirection * 100.0f, Color.red, 1.0f);
                Debug.DrawRay(hit.point, Vector3.up * 100.0f, Color.blue, 1.0f);

                bool minionAvailable = minionPool.Shoot(shootDirection); // Tells the closest minion to be shot. False if no minion close enough.

                if (minionAvailable)
                    player.Shoot(shootDirection); // Causes player to play a shooting animation and face in said direction momentarily. 
                
                //else Play Buzzer / Visual Indicator for failed shoot
            }
        }
    }
}
