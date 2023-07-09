using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawn : MonoBehaviour
{
    [Header("Spawning Values")]
    [SerializeField] public SpawningPoint[] spawnSpots;

    public GameObject ChickenPrefab;

    public float minSpawnTime = 3f;
    public float maxSpawnTime = 5f;

    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
        StartSpawn();
    }

    private void Update(){
        if (gameManager.intesitySetting == 2)
        {
            minSpawnTime = 2f;
            maxSpawnTime = 4f;
        }
        if (gameManager.intesitySetting == 3)
        {
            minSpawnTime = 2f;
            maxSpawnTime = 3f;
        }
        if (gameManager.intesitySetting == 4)
        {
            minSpawnTime = 1f;
            maxSpawnTime = 2f;
        }
        if (gameManager.intesitySetting == 5)
        {
            minSpawnTime = 0.5f;
            maxSpawnTime = 1.5f;
        }
    }

    private void StartSpawn()
    {
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        IEnumerator coroutine = WaitAndSpawn(spawnTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndSpawn(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);
        int selected = BasedRandom();
        SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);

        if (!gameManager.gameOver)
        {
            SpawnChicken(spawnSpots[selected]);
            if (gameManager.intesitySetting >= 1)
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            if (gameManager.intesitySetting >= 2) 
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            if (gameManager.intesitySetting >= 4)
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            if (gameManager.intesitySetting >= 5)
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            // Restart timer
            StartSpawn();
        }
    }

    private void SpawnChicken(SpawningPoint point)
    {
        Instantiate(ChickenPrefab, point.position, Quaternion.identity);
    }

    private int BasedRandom()
    {
        return Random.Range(1, spawnSpots.Length);
        // We could use spawnProbability in SpawningPoint object to create smarter probability
    }
}

[System.Serializable]
public class SpawningPoint 
{
    public Vector3 position;
    public float spawnProbability;
}
