using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string chickenContainer_Name = "ChickenContainer";
    [SerializeField] private string specialChickenContainer_Name = "SpecialChickenContainer";

    [Header("Spawning Values")]
    [SerializeField] public SpawningPoint[] spawnSpots;

    [Header("Per-Level Logic")]
    [SerializeField] private float startingX = -14.75f;
    [SerializeField] private float maxYHeight = 5f; //THESE DO NOT WORK ATM, WILL BE ADDRESSED IN THE FUTURE BUT IGNORE THIS FOR NOW PLZ TY
    [SerializeField] private float minYHeight = -5f; //SAME HERE

    [Header("Chicken Types")]
    [SerializeField] public GameObject chickenPrefab;

    private SoundManager soundManager;
    private GameObject chickenContainer;
    private GameObject specialChickenContainer;

    [HideInInspector] public ChickenWave currentWave;
    [HideInInspector] public List<SpecialChicken> specialChickens;
    public int waveChickenAmount;
    [HideInInspector] public float time = 0f;
    [HideInInspector] public bool waveEnded = false;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        chickenContainer = GameObject.Find(chickenContainer_Name);
        specialChickenContainer = GameObject.Find(specialChickenContainer_Name);
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
                SpawnAChicken(specialChickens[0].chicken, SelectSpawn(), true);
                specialChickens.RemoveAt(0);
            }
        }
    }

    private SpawningPoint SelectSpawn()
    {
        Vector3 spawn;
        SpawningPoint spawnPoint = new SpawningPoint();
        //if (specialChickens[0].topSpawn && specialChickens[0].bottomSpawn)
        //{
        //    float randomNum = Random.Range(-2f, 2f);
        //    spawn = new Vector3(-14.75f, randomNum, 0f);
        //}
        //else if (specialChickens[0].topSpawn)
        //{
        //    float randomNum = Random.Range(0, 6f);
        //    spawn = new Vector3(-14.75f, randomNum, 0f);
        //}
        //else if (specialChickens[0].bottomSpawn)
        //{
        //    float randomNum = Random.Range(0, -5f);
        //    spawn = new Vector3(-14.75f, randomNum, 0f);
        //}
        //else
        //{
        //    float randomNum = Random.Range(-5f, 6f);
        //    spawn = new Vector3(-14.75f, randomNum, 0f);
        //}

        float rangeY;
        if(specialChickens.Count > 0){
            if (specialChickens[0].topSpawn && specialChickens[0].bottomSpawn)
            {
                rangeY = Random.Range(minYHeight, maxYHeight);
            }
            else if (specialChickens[0].topSpawn)
            {
                rangeY = Random.Range(0, maxYHeight);
            }
            else if (specialChickens[0].bottomSpawn)
            {
                rangeY = Random.Range(minYHeight, 0);
            }
            else
            {
                rangeY = Random.Range(minYHeight, maxYHeight);
            }
        }
        else
        {
            rangeY = Random.Range(minYHeight, maxYHeight);
        }

        float roundedY = Mathf.Round(rangeY * 4) / 4;  // Round to the nearest multiple of 0.25

        spawn = new Vector3(startingX, roundedY, 0f);

        spawnPoint.position = spawn;
        return spawnPoint;
    }

    public void StandardChickenWave()
    {
        if (!waveEnded)
        {
            //float timeBetweenSpawns = currentWave.roundTime / currentWave.standardChickenAmounts;
            //SpawningPoint point = spawnSpots[Random.Range(0, spawnSpots.Length - 1)];

            //IEnumerator coroutine = WaitAndSpawnChicken(timeBetweenSpawns);
            //StartCoroutine(coroutine);

            //SpawnAChicken(chickenPrefab, point);
            //waveChickenAmount--;

            float timeBetweenSpawns = currentWave.roundTime / currentWave.standardChickenAmounts;

            IEnumerator coroutine = WaitAndSpawnChicken(timeBetweenSpawns);
            StartCoroutine(coroutine);

            // Generate a random Y value based on your constraints
            float rangeY = Random.Range(minYHeight, maxYHeight);
            float roundedY = Mathf.Round(rangeY * 4) / 4; // Round to the nearest multiple of 0.25

            // Create a new Vector3 with the fixed startingX, random Y, and 0 for Z
            Vector3 spawn = new Vector3(startingX, roundedY, 0f);

            SpawningPoint spawnPoint = new SpawningPoint();
            spawnPoint.position = spawn;

            // Assuming SpawnAChicken accepts a Vector3 for the spawn position
            SpawnAChicken(chickenPrefab, spawnPoint, false);

            waveChickenAmount--;
        }
    }

    private IEnumerator WaitAndSpawnChicken(float time)
    {
        yield return new WaitForSeconds(time);
        if (waveChickenAmount > 0)
            StandardChickenWave();
    }

    private void SpawnAChicken(GameObject chicken, SpawningPoint point, bool isSpecial = false)
    {
        if(isSpecial){
            chicken = Instantiate(chicken, point.position, Quaternion.identity, specialChickenContainer.transform);
        }
        else{
            chicken = Instantiate(chicken, point.position, Quaternion.identity, chickenContainer.transform);
        }
        if(chicken.GetComponent<ChickenMovement>() != null){
            chicken.GetComponent<ChickenMovement>().chickenIntesity = currentWave.chickenIntesity;
            chicken.GetComponent<ChickenMovement>().minYHeight = minYHeight;
            chicken.GetComponent<ChickenMovement>().maxYHeight = maxYHeight;
        }

        //if (soundManager != null)
        //    soundManager.PlayRandomChicken();
    }
    public void AlternateSpawnAChicken(GameObject chicken){
        SpawnAChicken(chicken, SelectSpawn());
    }

}

[System.Serializable]
public class SpawningPoint
{
    public SpawningPoint() { }
    public Vector3 position;

}
