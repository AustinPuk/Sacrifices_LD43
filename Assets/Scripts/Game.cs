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
    public EnemyPool enemyPool;

    [SerializeField]
    public Shooter shooter;

    [SerializeField]
    List<Vector2> waveInfo;

    [SerializeField]
    EndScreen endScreen;

    public bool isPaused;

    public int minionsDead = 0;

    public int waveCurrent = 1;

    public int MaxWaves { get { return waveInfo.Count; } }

    int enemiesLeft;

    void Awake ()
    {
        if (!game)
            game = this;

        isPaused = true;

        //StartGame();
    }

    public void StartGame()
    {
        isPaused = false;
        waveCurrent = 1;
        minionsDead = 0;
        player.Restart();
        minionPool.Restart();
        enemyPool.Restart();
        StartWave();
    }

    void StartWave()
    {
        Vector2 wave = waveInfo[waveCurrent - 1];
        enemyPool.SpawnWave((int) wave.x, wave.y);
        enemiesLeft = (int) wave.x;
    }

    public void WaveEnd()
    {
        waveCurrent++;
        if (waveCurrent <= waveInfo.Count)
            StartWave();
        else
        {
            waveCurrent--;
            isPaused = true;
            enemyPool.StopSpawn();
            endScreen.SetWinScreen();
        }
    }

    public void PlayerDies()
    {
        isPaused = true;
        enemyPool.StopSpawn();
        endScreen.SetLoseScreen();
    }

    public void MinionDies()
    {
        minionsDead++;
    }

    public void EnemyDies()
    {
        enemiesLeft--;

        if (enemiesLeft <= 0)
            WaveEnd();
    }
}