using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    Image healthGauge;

    // Use this for initialization
    void Start ()
    {
        healthGauge.fillAmount = 1.0f;
    }
    
    // Update is called once per frame
    void Update ()
    {
        healthGauge.fillAmount = Game.game.player.HealthPercent;
    }
}