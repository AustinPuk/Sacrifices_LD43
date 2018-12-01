﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionPool : MonoBehaviour
{
    [SerializeField]
    GameObject player; // TODO : Pass this from Game object instead.

    // Minimum distance a minion needs to be from the player to be shot
    [SerializeField]
    float minimumShootDistance = 2.0f;

    List<Minion> minions;
    List<Minion> following;

    void Start ()
    {
        minions = new List<Minion>(GetComponentsInChildren<Minion>());
        following = new List<Minion>(GetComponentsInChildren<Minion>());
    }
    
    // Update is called once per frame
    void Update ()
    {
        
    }

    public bool Shoot(Vector3 shootDirection)
    {
        // Finds minion closest to the player (only check minions that are following player)
        Minion closestMinion = null;
        float closestDist = minimumShootDistance;
        foreach (Minion minion in following)
        {
            float dist = Vector3.SqrMagnitude(player.transform.position - minion.transform.position);
            if ( dist < closestDist)
            {
                closestDist = dist;
                closestMinion = minion;
            }
        }

        if (closestMinion == null)
            return false;  // Shooting failed.

        Debug.Log("Following Before : " + following.Count);
        following.Remove(closestMinion);
        Debug.Log("Following After : " + following.Count);
        closestMinion.Shoot(shootDirection);
        return true;
    }
}