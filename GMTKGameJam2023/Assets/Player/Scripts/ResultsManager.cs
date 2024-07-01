using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 leaderPosition;
    private LeaderboardManager leaderboardManager;
    private CameraMenuMove mainCamera;
    [SerializeField] private GameObject[] chickens;
    [SerializeField] private GameObject standardChicken;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private ChickenSpawn chickenSpawn;
    private int chickenAmount = 0;

    void Awake()
    {
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
        mainCamera = FindObjectOfType<CameraMenuMove>();
    }

    void Start()
    {
        leaderboardManager.UploadToLeaderboardAndUpdate(Points.playerScore);
        ResetSpawnChicken();
    }

    public void GoToLeaderboard(){
        mainCamera.targetPos = leaderPosition;
    }

    public void GoToStart(){
        mainCamera.targetPos = startPosition;
    }    

    public void ResetSpawnChicken(){
        IEnumerator coroutine = SpawnChickenForResults(timeBetweenSpawns);
        StartCoroutine(coroutine);
    }

    IEnumerator SpawnChickenForResults(float timeToSpawn)
    {
        int chickenNum = 0;
        if(chickenAmount % 5 ==0) chickenNum = Random.Range(1, 8);
        chickenSpawn.AlternateSpawnAChicken(chickens[chickenNum]);
        // Debug.Log("Chicken: " + chickens[chickenNum].name);
        chickenAmount++;
        yield return new WaitForSeconds(timeToSpawn);
        ResetSpawnChicken();
    }
}
