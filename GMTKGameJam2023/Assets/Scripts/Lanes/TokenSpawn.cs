using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawn : MonoBehaviour
{

    [SerializeField] 
    public Transform[] spawnSpots;
    public GameObject[] tokenPrefabs;

    public float minSpawnTime = 5f;
    public float maxSpawnTime = 25f;

    // Start is called before the first frame update
    void Start()
    {
        // foreach (Transform tansform in spawnSpots)
        // {
        //     Instantiate(tokenPrefabs[0], transform.position, Quaternion.identity);
        // }
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
