using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : No longer pooling. Rename to something else.
public class EnemyPool : MonoBehaviour
{
    [SerializeField]
    GameObject spawnLocationsObject;

    [SerializeField]
    GameObject enemy;

    List<Vector3> spawnLocations;
    Coroutine spawner;

    public void Restart()
    {
        StopSpawn();
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Enemy>() != null)
                Destroy(child.gameObject); // TODO : Check if it's safe to destroy in for loop
        }
    }

    public bool ConfirmEnemiesAllDefeated()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Enemy>() != null)
            {
                Debug.Log("ENEMY STILL REMAINING");
                return false;
            }
        }
        return true;
    }

    public void SpawnWave(int numEnemies, float spawnTimer)
    {
        spawnLocations = new List<Vector3>();
        foreach (Transform child in spawnLocationsObject.transform)
        {
            spawnLocations.Add(child.position);
        }
        spawner = StartCoroutine(SpawnTimer(numEnemies, spawnTimer));
    }

    public void StopSpawn()
    {
        if (spawner != null)
            StopCoroutine(spawner);
    }

    IEnumerator SpawnTimer(int numEnemies, float spawnTimer)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemy, spawnLocations[Random.Range(0, spawnLocations.Count - 1)], Quaternion.identity, transform);
            newEnemy.GetComponent<Enemy>().Spawned();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
