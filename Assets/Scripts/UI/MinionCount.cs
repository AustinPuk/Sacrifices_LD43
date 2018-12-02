using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionCount : MonoBehaviour
{
    [SerializeField]
    Text counter;

    [SerializeField]
    Image gauge;

    // Use this for initialization
    void Start()
    {
        counter.text = "0";
        gauge.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = Game.game.minionPool.FollowerCount.ToString();
        gauge.fillAmount = Game.game.minionPool.FollowerPercentage;
    }
}