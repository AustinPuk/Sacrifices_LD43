using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : No longer pooling. Rename to something else.
public class EnemyPool : MonoBehaviour
{
    [SerializeField]
    GameObject spawnLocationsObject;

    [SerializeField]
    float spawnTimer;

    [SerializeField]
    GameObject enemy;

    List<Vector3> spawnLocations;

    Coroutine spawner;

    void Start()
    {
        spawnLocations = new List<Vector3>();
        foreach (Transform child in spawnLocationsObject.transform)
        {
            spawnLocations.Add(child.position);
        }
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
            GameObject newEnemy = Instantiate(enemy, spawnLocations[Random.Range(0, spawnLocations.Count - 1)], Quaternion.identity, transform);
            newEnemy.GetComponent<Enemy>().Spawned();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
