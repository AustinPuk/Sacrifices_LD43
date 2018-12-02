using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // This should be the only singleton in the game
    public static Game game;

    [SerializeField]
    public Player player; // TODO :Not make these public

    [SerializeField]
    public MinionPool minionPool;

    [SerializeField]
    public Shooter shooter;

    public bool isRunning = true;

    public int minionsDead = 0;

    public int waveCurrent = 1;

    void Start ()
    {
        if (!game)
            game = this;
    }

    void Update ()
    {
        
    }


    public void MinionDies()
    {
        minionsDead++;
    }
}