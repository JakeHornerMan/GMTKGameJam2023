using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawn : MonoBehaviour
{
    [Header("Spawning Values")]
    [SerializeField] public SpawningPoint[] spawnSpots;

    [Header("Chicken Types")]
    [SerializeField] public GameObject ChickenPrefab;
    private SoundManager soundManager;

    [HideInInspector] public ChickenWave currentWave;
    [HideInInspector] public List<SpecialChicken> specialChickens;
    [HideInInspector] public int waveChickenAmount;
    [HideInInspector] public float time = 0f;
    [HideInInspector] public bool waveEnded = false;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void FixedUpdate() {
        if(!waveEnded){
            WaveTime();
        }
    }

    public void SetNewWave(ChickenWave wave){
        currentWave = wave;
        specialChickens = wave.specialChickens;
        specialChickens.Sort((obj1,obj2)=>obj1.timeToSpawn.CompareTo(obj2.timeToSpawn));
        waveEnded = false;
        time = 0f; 

        if(currentWave.standardChickenAmounts > 0) 
            StandardChickenNewWave();
    }

    private void WaveTime(){
        time += Time.deltaTime;
        if (time < currentWave.roundTime)
        {
            if(specialChickens.Count > 0 && time >= specialChickens[0].timeToSpawn){
                SpawnAChicken(specialChickens[0].chicken);
                specialChickens.RemoveAt(0);
            }
        }
        if(time > currentWave.roundTime){
            waveEnded = true;
        }
    }

    public void StandardChickenNewWave(){
        if(!waveEnded){
            float timeBetweenSpawns = currentWave.roundTime/currentWave.standardChickenAmounts;
            SpawnAChicken(ChickenPrefab);

            waveChickenAmount--;

            IEnumerator coroutine = WaitAndSpawnChicken(timeBetweenSpawns);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator WaitAndSpawnChicken(float time)
    {
        yield return new WaitForSeconds(time);
        if(waveChickenAmount <= 0) 
            StandardChickenNewWave();
    }

    private void SpawnAChicken(GameObject chicken)
    {
        SpawningPoint point = spawnSpots[Random.Range(0, spawnSpots.Length-1)];
        chicken = Instantiate(chicken, point.position, Quaternion.identity);
        chicken.GetComponent<ChickenMovement>().chickenIntesity = currentWave.chickenIntesity;
        if (soundManager != null)
            soundManager.PlayRandomChicken();
    }

}

[System.Serializable]
public class SpawningPoint
{
    public Vector3 position;
    public float spawnProbability;
}
