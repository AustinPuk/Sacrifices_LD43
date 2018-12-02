﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    Text winText;

    [SerializeField]
    Text loseText;

    [SerializeField]
    Text minionsDied;

    public void SetLoseScreen()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(true);
        minionsDied.text = Game.game.minionsDead.ToString();
    }

    public void SetWinScreen()
    {
        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
        minionsDied.text = Game.game.minionsDead.ToString();
    }
}
