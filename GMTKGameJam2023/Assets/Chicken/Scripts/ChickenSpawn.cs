using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawn : MonoBehaviour
{
    [Header("Spawning Values")]
    [SerializeField] public SpawningPoint[] spawnSpots;

    public GameObject ChickenPrefab;
    public GameObject GoldenChickenPrefab;

    public float minSpawnTime = 3f;
    public float maxSpawnTime = 5f;

    public int goldenChickenOdds = 50;

    private GameManager gameManager;
    private SoundManager soundManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        SpawnChicken(spawnSpots[Random.Range(0, spawnSpots.Length-1)]);
        StartSpawn();
    }

    public void UpdateIntensity(int intensity)
    {
        if (intensity == 2)
        {
            minSpawnTime = 2f;
            maxSpawnTime = 4f;
            goldenChickenOdds = 45;
        }
        if (intensity == 3)
        {
            minSpawnTime = 2f;
            maxSpawnTime = 3f;
            goldenChickenOdds = 40;
        }
        if (intensity == 4)
        {
            minSpawnTime = 1f;
            maxSpawnTime = 2f;
            goldenChickenOdds = 30;
        }
        if (intensity == 5)
        {
            minSpawnTime = 0.5f;
            maxSpawnTime = 1.5f;
            goldenChickenOdds = 15;
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

        int randomNum = Random.Range(0, goldenChickenOdds);

        if (randomNum == 0)
        {
            SpawnGoldenChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
        }
        else
        {
            SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
        }

        if (!gameManager.gameOver)
        {
            SpawnChicken(spawnSpots[selected]);
            if (gameManager.intensitySetting >= 1)
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            if (gameManager.intensitySetting >= 2)
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            if (gameManager.intensitySetting >= 4)
            {
                SpawnChicken(spawnSpots[Random.Range(1, spawnSpots.Length)]);
            }
            if (gameManager.intensitySetting >= 5)
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
        if (soundManager != null)
            soundManager.PlayRandomChicken();
    }

    private void SpawnGoldenChicken(SpawningPoint point)
    {
        Instantiate(GoldenChickenPrefab, point.position, Quaternion.identity);
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
