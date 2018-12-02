using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionBar : MonoBehaviour
{
    [SerializeField]
    Image gauge;

    // Use this for initialization
    void Start()
    {
        gauge.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        gauge.fillAmount = 1.0f - Game.game.shooter.CooldownPercent;
    }
}