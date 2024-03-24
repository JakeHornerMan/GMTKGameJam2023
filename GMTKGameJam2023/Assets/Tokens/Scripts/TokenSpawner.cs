using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
     [Header("References")]
    [SerializeField] private string tokenContainer_Name = "TokenContainer";
    private GameManager gameManager;
    [SerializeField] public GameObject tokenPrefab;
    [SerializeField] private GameObject[] tokenPrefabs;
    private GameObject tokenContainer;

    private GameObject[] roadLanes;
    private GameObject[] grassLanes;
    private GameObject[] busLanes;
    private GameObject[] waterLanes;
    private GameObject[] pavementLanes;

    [HideInInspector] private ChickenWave currentWave;
    [HideInInspector] private int tokenAmountInWave;
    [HideInInspector] private bool waveEnded;

    public List<GameObject> allLanes;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tokenContainer = GameObject.Find(tokenContainer_Name);
    }

    private void Start()
    {
        
    }

    public void GetPotentialRoads()
    {
        foreach (Car car in gameManager.carsInLevel)
        {
            for(int i =0 ; i <= car.placeableLaneTags.Count -1; i++)
            {
                if(!car.ignoreTokens){

                    if(car.placeableLaneTags[i].Contains("Road")){
                        roadLanes = GameObject.FindGameObjectsWithTag("Road");
                        AddToAllLanes(roadLanes);
                    }
                        
                    if(car.placeableLaneTags[i].Contains("Grass")){
                        grassLanes = GameObject.FindGameObjectsWithTag("Grass");
                        AddToAllLanes(grassLanes);
                    }
                        
                    if(car.placeableLaneTags[i].Contains("Bus Lane")){
                        busLanes = GameObject.FindGameObjectsWithTag("Bus Lane");
                        AddToAllLanes(busLanes);
                    }
                        
                    if(car.placeableLaneTags[i].Contains("Water")){
                        waterLanes = GameObject.FindGameObjectsWithTag("Water");
                        AddToAllLanes(waterLanes);
                    }
                        
                    if(car.placeableLaneTags[i].Contains("Pavement")){
                        pavementLanes = GameObject.FindGameObjectsWithTag("Pavement");
                        AddToAllLanes(pavementLanes);
                    }
                }
                    
            }
        }
    }

    private void AddToAllLanes(GameObject[] laneArray)
    {
        if(laneArray.Length > 0){
            foreach (GameObject value in laneArray)
            {
                allLanes.Add(value);
            }
        }
    }

    public void SetNewWave(ChickenWave wave)
    {
        if(allLanes.Count == 0){
            GetPotentialRoads();
        }
        currentWave = wave;
        tokenAmountInWave = wave.coinAmount;
        waveEnded = false;

        if (tokenAmountInWave > 0)
            StandardTokenWave();
    }

    public void StandardTokenWave()
    {
        if (!waveEnded)
        {
            float timeBetweenSpawns = currentWave.roundTime / currentWave.coinAmount;

            IEnumerator coroutine = WaitAndSpawnToken(timeBetweenSpawns);
            StartCoroutine(coroutine);

            SpawnToken(tokenPrefab, choseLane());
            tokenAmountInWave--;
        }
    }

    private IEnumerator WaitAndSpawnToken(float time)
    {
        yield return new WaitForSeconds(time);
        if (tokenAmountInWave > 0)
            StandardTokenWave();
    }

    public Transform choseLane()
    {
        // int randomInt = Random.Range(0, allLanes.Count);
        //Causing an index error, fix this
        return allLanes[Random.Range(0, allLanes.Count)].transform;
    }

    private void SpawnToken(GameObject token, Transform point)
    {
        Vector3 pos = point.position;
        float randomFloat = Random.Range(-4f, 4f);
        pos.y += randomFloat;
        if (gameManager.isGameOver) return;
        Instantiate(token, pos, Quaternion.identity, tokenContainer.transform);
    }
}
