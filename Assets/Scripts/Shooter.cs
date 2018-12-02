using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On mouse clicks, find direction to shoot and send signal to MinionPool & Player

public class Shooter : MonoBehaviour
{
    [SerializeField]
    float cooldown;

    bool onCooldown;
    float cooldownTimer;

    public float CooldownPercent { get { return cooldownTimer / cooldown; } }

    void Start ()
    {
        onCooldown = false;
    }

    void Update ()
    {
        if (Game.game.isPaused)
            return;

        CooldownCheck();
        ProcessMouse();
    }

    void ProcessMouse()
    {

        Player player = Game.game.player;
        MinionPool minionPool = Game.game.minionPool;

        if (Input.GetMouseButtonDown(0) &&  !onCooldown)
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
                {
                    player.Shoot(shootDirection); // Causes player to play a shooting animation and face in said direction momentarily. 
                    onCooldown = true;
                    cooldownTimer = cooldown;
                }
                
                //else Play Buzzer / Visual Indicator for failed shoot
            }
        }
    }

    void CooldownCheck()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0.0f)
        {
            cooldownTimer = 0.0f;
            onCooldown = false;
        }
    }
}
