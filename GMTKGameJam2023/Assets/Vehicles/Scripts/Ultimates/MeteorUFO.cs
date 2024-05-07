using System.Collections;
using UnityEngine;

/// <summary>
/// Drops meteors at random locations on level, while moving horizontally.
/// </summary>
public class MeteorUFO : MonoBehaviour
{
    [Header("UFO Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float xDestroyThreshold = 25f;

    [Header("Random Area Settings")]
    [SerializeField] private float maxY = 7;
    [SerializeField] private float minY = -7;
    [SerializeField] private float maxX = 10;
    [SerializeField] private float minX = -10;

    [Header("Meteor Options")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private int numMeteors = 10;
    [SerializeField] private float meteorSpawnDelay = 0.5f;

    private bool meteorShowerStarted = false;

    private void Start()
    {
        StartCoroutine(MoveUFO());
    }

    private void Update()
    {
        if (transform.position.x > xDestroyThreshold)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveUFO()
    {
        Vector3 direction = Vector3.right;

        while (true)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if (!meteorShowerStarted && transform.position.x > -10)
            {
                meteorShowerStarted = true;
                StartCoroutine(SpawnMeteors());
            }

            yield return null;
        }
    }

    private IEnumerator SpawnMeteors()
    {
        for (int i = 0; i < numMeteors; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );
            Instantiate(
                meteorPrefab,
                randomPos,
                Quaternion.identity
            );

            yield return new WaitForSeconds(meteorSpawnDelay);
        }
    }
}
