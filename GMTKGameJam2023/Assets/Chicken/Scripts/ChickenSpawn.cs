using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string chickenContainer_Name = "ChickenContainer";

    [Header("Spawning Values")]
    [SerializeField] public SpawningPoint[] spawnSpots;

    [Header("Chicken Types")]
    [SerializeField] public GameObject chickenPrefab;

    private SoundManager soundManager;
    private GameObject chickenContainer;

    [HideInInspector] public ChickenWave currentWave;
    [HideInInspector] public List<SpecialChicken> specialChickens;
    public int waveChickenAmount;
    [HideInInspector] public float time = 0f;
    [HideInInspector] public bool waveEnded = false;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        chickenContainer = GameObject.Find(chickenContainer_Name);
    }

    private void FixedUpdate()
    {
        if (!waveEnded)
        {
            WaveTime();
        }
    }

    public void SetNewWave(ChickenWave wave)
    {
        Debug.Log("New Wave");
        currentWave = wave;

        if(wave.specialChickens != null){
            specialChickens = wave.specialChickens;
            specialChickens.Sort((obj1, obj2) => obj1.timeToSpawn.CompareTo(obj2.timeToSpawn));
        }
        
        waveEnded = false;
        time = 0f;
        waveChickenAmount = wave.standardChickenAmounts;

        if (currentWave.standardChickenAmounts > 0)
            StandardChickenWave();
    }

    private void WaveTime()
    {
        time += Time.deltaTime;
        if (time > currentWave.roundTime)
        {
            waveEnded = true;
        }
        if (time < currentWave.roundTime)
        {
            if (specialChickens.Count > 0 && time >= specialChickens[0].timeToSpawn)
            {
                SpawnAChicken(specialChickens[0].chicken, SelectSpawn());
                specialChickens.RemoveAt(0);
            }
        }
    }

    private SpawningPoint SelectSpawn()
    {
        Vector3 spawn;
        SpawningPoint spawnPoint = new SpawningPoint();
        if (specialChickens[0].topSpawn && specialChickens[0].bottomSpawn)
        {
            float randomNum = Random.Range(-2f, 2f);
            spawn = new Vector3(-14.75f, randomNum, 0f);
        }
        else if (specialChickens[0].topSpawn)
        {
            float randomNum = Random.Range(0, 6f);
            spawn = new Vector3(-14.75f, randomNum, 0f);
        }
        else if (specialChickens[0].bottomSpawn)
        {
            float randomNum = Random.Range(0, -5f);
            spawn = new Vector3(-14.75f, randomNum, 0f);
        }
        else
        {
            float randomNum = Random.Range(-5f, 6f);
            spawn = new Vector3(-14.75f, randomNum, 0f);
        }
        spawnPoint.position = spawn;
        return spawnPoint;
    }

    public void StandardChickenWave()
    {
        if (!waveEnded)
        {
            float timeBetweenSpawns = currentWave.roundTime / currentWave.standardChickenAmounts;
            SpawningPoint point = spawnSpots[Random.Range(0, spawnSpots.Length - 1)];

            IEnumerator coroutine = WaitAndSpawnChicken(timeBetweenSpawns);
            StartCoroutine(coroutine);

            SpawnAChicken(chickenPrefab, point);
            waveChickenAmount--;
        }
    }

    private IEnumerator WaitAndSpawnChicken(float time)
    {
        yield return new WaitForSeconds(time);
        if (waveChickenAmount > 0)
            StandardChickenWave();
    }

    private void SpawnAChicken(GameObject chicken, SpawningPoint point)
    {
        chicken = Instantiate(chicken, point.position, Quaternion.identity, chickenContainer.transform);
        chicken.GetComponent<ChickenMovement>().chickenIntesity = currentWave.chickenIntesity;
        if (soundManager != null)
            soundManager.PlayRandomChicken();
    }

}

[System.Serializable]
public class SpawningPoint
{
    public SpawningPoint() { }
    public Vector3 position;

}
