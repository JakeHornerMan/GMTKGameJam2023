using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawn : MonoBehaviour
{
    [SerializeField]
    public SpawningPoint[] spawnSpots;

    public GameObject ChickenPrefab;

    public float minSpawnTime = 3f;
    public float maxSpawnTime = 6f;

    void Start()
    {
        spawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
        startSpawn();
    }

    void startSpawn(){
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        IEnumerator coroutine = WaitAndSpawn(spawnTime);
        StartCoroutine(coroutine);
    }

    IEnumerator WaitAndSpawn(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);
        int selected = basedRandom();
        // Debug.Log(basedRandom());
        spawnChicken(spawnSpots[selected]);
        //restart timer
        startSpawn();
    }

    void spawnChicken(SpawningPoint point){
        // Debug.Log("Spawning Chickens");
        // foreach (SpawningPoint point in spawnSpots){
        //     Instantiate(ChickenPrefab, point.position, Quaternion.identity);
        // }
        Instantiate(ChickenPrefab, point.position, Quaternion.identity);
    }

    int basedRandom(){
        return Random.Range(1, spawnSpots.Length);
        //we could use spawnProbability in SpawningPoint object to create smarter probability
    }
}
[System.Serializable]
public class SpawningPoint {
    public Vector3 position;
    public float spawnProbability;
}
