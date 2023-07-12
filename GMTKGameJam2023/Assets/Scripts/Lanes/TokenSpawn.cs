using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawn : MonoBehaviour
{
    [Header("Token Spawn Values")]
    [SerializeField] public Transform[] spawnSpots;
    [SerializeField] public GameObject[] tokenPrefabs;

    public float minSpawnTime = 5f;
    public float maxSpawnTime = 25f;

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
        StartSpawn();
    }

    private void SpawnToken(Transform point)
    {
        Instantiate(tokenPrefabs[0], point.position, Quaternion.identity);
    }
}
