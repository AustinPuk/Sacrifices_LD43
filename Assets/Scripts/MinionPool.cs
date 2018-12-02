using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionPool : MonoBehaviour
{
    // Minimum distance a minion needs to be from the player to be shot
    [SerializeField]
    float minimumShootDistance;

    [SerializeField]
    int maxFollowers;

    [SerializeField]
    Transform spawnLocation;

    [SerializeField]
    GameObject minionPrefab;

    List<Minion> minions;
    List<Minion> following;

    public int FollowerCount { get { return following.Count; } }
    public float FollowerPercentage { get { return (float) FollowerCount / (float) maxFollowers; } }

    void Start ()
    {
        following = new List<Minion>();
    }
    
    // Update is called once per frame
    void Update ()
    {
    }

    public void Gather()
    {
        if (following.Count >= maxFollowers)
            return;

        GameObject newMinionObject = Instantiate(minionPrefab, spawnLocation.position, Quaternion.identity, transform);
        Minion newMinion = newMinionObject.GetComponent<Minion>();
        following.Add(newMinion);
        newMinion.Follow();
        return;
    }

    public bool Shoot(Vector3 shootDirection)
    {
        // Finds minion closest to the player (only check minions that are following player)
        Minion closestMinion = null;
        float closestDist = minimumShootDistance * minimumShootDistance;
        foreach (Minion minion in following)
        {
            float dist = Vector3.SqrMagnitude(Game.game.player.transform.position - minion.transform.position);
            if ( dist < closestDist)
            {
                closestDist = dist;
                closestMinion = minion;
            }
        }

        if (closestMinion == null)
            return false;  // Shooting failed.
        else
        {
            following.Remove(closestMinion);
            closestMinion.Shoot(shootDirection);
            return true;
        }
    }
}
