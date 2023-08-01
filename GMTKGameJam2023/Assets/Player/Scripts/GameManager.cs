using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultsUI resultsUI;

    [Header("ChickenWaves")]
    [SerializeField] public List<ChickenWave> waves;

    [Header("Gameplay Settings")]
    [SerializeField] public float startTime = 180f;
    [SerializeField] public bool devMode = false;
    [SerializeField]  public int intensitySetting = 0;
    [SerializeField]  public bool isGameOver = false;
    [SerializeField]  public int failureChickenAmount = 10;
    [SerializeField]  public int lostChicenScore = 1000;

    [HideInInspector] public int safelyCrossedChickens = 0;
    [HideInInspector] public int killCount = 0;
    [HideInInspector] public int playerScore = 0;
    [SerializeField] public int tokens = 0;
    [HideInInspector] public int totalTokens = 0;
    [HideInInspector] public float time = 120f;
    [HideInInspector] public string currentRanking = "Animal Lover";
    [HideInInspector] public bool endSound = false;
    [HideInInspector] public int waveNumber = 0;

    private SoundManager soundManager;
    private Pause pause;
    private ChickenSpawn chickenSpawn;
    private SceneFader sceneFader;
    private InterfaceManager interfaceManager;
    private CameraShaker cameraShaker;

    private void Awake()
    {
        pause = FindObjectOfType<Pause>();
        soundManager = FindObjectOfType<SoundManager>();
        chickenSpawn = GetComponent<ChickenSpawn>();
        interfaceManager = GetComponent<InterfaceManager>();
        sceneFader = FindObjectOfType<SceneFader>();
        cameraShaker = FindObjectOfType<CameraShaker>();
    }

    private void Start()
    {
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        //tokens = 0;
        totalTokens = 0;

        SetGameTime();
        time = startTime;

        if(waves.Count != 0)
            SettingWaveInChickenSpawn();

        if(isGameOver)
            MissedChickensWave();

    }

    private void SetGameTime(){
        float gameTime = 0f;
        if(waves.Count != 0){
            foreach (ChickenWave value in waves)
            {
            gameTime = gameTime + value.roundTime;
            }
            startTime = gameTime;
        }
    }

    private void SettingWaveInChickenSpawn(){
        // Debug.Log("Current Wave: "+ waveNumber);
        
        ChickenWave currentWave = waves[waveNumber];
        IncreaseIntensity(currentWave.wavePrompt);

        chickenSpawn.SetNewWave(currentWave);
        
        IEnumerator coroutine = WaitAndNextWave(currentWave.roundTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndNextWave(float time)
    {
        yield return new WaitForSeconds(time);
        waveNumber++;
        if(waveNumber < waves.Count)
            SettingWaveInChickenSpawn();
    }

    private void MissedChickensWave()
    {
        ChickenWave endWave = new ChickenWave();
        endWave.roundTime = 10f;
        endWave.standardChickenAmounts = Points.safelyCrossedChickens;
        endWave.wavePrompt = "";
        endWave.specialChickens = new List<SpecialChicken>();
        waves.Add(endWave);
        SettingWaveInChickenSpawn();
    }

    private void FixedUpdate()
    {
        if(!isGameOver){
            SetTime();
        }
        // UpdateRankings();
    }

    private void SetTime()
    {
        if (time > 0)
            time -= Time.deltaTime;
        
        if (time <= 0 || failureChickenAmount == safelyCrossedChickens)
        {
            isGameOver = true;
            UpdateRankings();
            HandleResults();
        }
        if (time <= 18f)
        {
            if(soundManager != null){
                soundManager.PlayLastSeconds();
                endSound = true;
            }
        }
    }

    public void SafelyCrossedChicken(){
        safelyCrossedChickens++;
        RemovePlayerScore(lostChicenScore * safelyCrossedChickens);
        soundManager.PlayMissedChicken();
        StartCoroutine(cameraShaker.Shake(0.25f, -0.5f));
    }

    public void AddPlayerScore(int addAmount){
        playerScore += addAmount;
    }

    public void RemovePlayerScore(int removeAmount){
        playerScore -= removeAmount;
    }

    private void UpdateRankings()
    {
        // Update Rankings
        switch (killCount)
        {
            case > 500:
                currentRanking = "Master Chicken Assassin";
                break;
            case > 250:
                currentRanking = "Sadist";
                break;
            case > 150:
                currentRanking = "KFC Worker";
                break;
            case > 100:
                currentRanking = "Vehicularly Sus";
                break;
            case > 60:
                currentRanking = "Accidents Happen";
                break;
            case > 30:
                currentRanking = "Traffic Obeyer";
                break;
            case 0:
                currentRanking = "Animal Lover";
                break;
        }
        if(failureChickenAmount == safelyCrossedChickens){
            currentRanking = "You Failed";
        }
    }

    private void IncreaseIntensity(string speedUpText)
    {
        intensitySetting++;

        if (soundManager != null)
            soundManager.PlayGameSpeed();
        if (interfaceManager != null)
            interfaceManager.ShowSpeedUpText(speedUpText);
    }

    private void HandleResults()
    {
        Points.currentRanking = currentRanking;
        Points.killCount = killCount;
        Points.safelyCrossedChickens = safelyCrossedChickens;
        Points.playerScore = playerScore;
        sceneFader.FadeToResults();
    }
}

[System.Serializable]
public class ChickenWave
{
    public ChickenWave(){}
    public float roundTime;
    public String wavePrompt;
    public int standardChickenAmounts;
    public int chickenIntesity = 0;
    public List<SpecialChicken> specialChickens;
}

[System.Serializable]
public class SpecialChicken
{
    public float timeToSpawn;
    public GameObject chicken;
    public bool topSpawn;
    public bool bottomSpawn;
    
}