using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawn : MonoBehaviour
{
    [Header("Token Spawn Values")]
    [SerializeField] private Transform[] spawnSpots;
    [SerializeField] private GameObject[] tokenPrefabs;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 25f;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        StartSpawn();
    }

    private void StartSpawn()
    {
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        IEnumerator coroutine = WaitAndSpawn(spawnTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndSpawn(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        int selected = Random.Range(1, spawnSpots.Length);
        SpawnToken(spawnSpots[selected]);

        // Restart timer
        if (gameManager.isGameOver) yield return null;
        StartSpawn();
    }

    public void SpawnToken(Transform point)
    {
        if (gameManager.isGameOver) return;
        Instantiate(tokenPrefabs[0], point.position, Quaternion.identity);
    }
}
