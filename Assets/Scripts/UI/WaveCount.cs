using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCount : MonoBehaviour
{
    [SerializeField]
    Text counter;

    [SerializeField]
    Text maxWave;

    // Use this for initialization
    void Start()
    {
        counter.text = "1";
        maxWave.text = Game.game.MaxWaves.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = Game.game.waveCurrent.ToString();
    }
}