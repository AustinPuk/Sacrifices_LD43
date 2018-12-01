using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField]
    List<Vector3> spawnLocations;

    [SerializeField]
    float spawnTimer;

    List<Enemy> enemies;

    Coroutine spawner;

    void Start()
    {
        enemies = new List<Enemy>(GetComponentsInChildren<Enemy>());
        spawner = StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update ()
    {
        
    }

    IEnumerator SpawnTimer()
    {
        while(true)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsInactive)
                {
                    enemy.Spawn(spawnLocations[Random.Range(0, spawnLocations.Count - 1)]);
                    break;
                }
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
