using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultsUI resultsUI;

    [Header("ChickenWaves")]
    [SerializeField] public ChickenWave[] waves;

    [Header("Gameplay Settings")]
    [SerializeField] public float startTime = 180f;
    [SerializeField] public bool devMode = false;
    [SerializeField] public int intensitySetting = 0;

    [HideInInspector] public int safelyCrossedChickens = 0;
    [HideInInspector] public int killCount = 0;
    [HideInInspector] public int playerScore = 0;
    [HideInInspector] public int tokens = 0;
    [HideInInspector] public int totalTokens = 0;
    [HideInInspector] public float time = 120f;
    [HideInInspector] public string currentRanking = "Animal Lover";
    [HideInInspector] public bool endSound = false;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public int waveNumber = 0;

    private SoundManager soundManager;
    private Pause pause;
    private ChickenSpawn chickenSpawn;
    private SceneFader sceneFader;
    private InterfaceManager interfaceManager;

    private void Awake()
    {
        pause = FindObjectOfType<Pause>();
        soundManager = FindObjectOfType<SoundManager>();
        chickenSpawn = GetComponent<ChickenSpawn>();
        interfaceManager = GetComponent<InterfaceManager>();
        sceneFader = FindObjectOfType<SceneFader>();
    }

    private void Start()
    {
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        tokens = 0;
        totalTokens = 0;

        SetGameTime();
        time = startTime;

        if(waves.Length != 0)
            SettingWaveInChickenSpawn();
    }

    private void SetGameTime(){
        float gameTime = 0f;
        if(waves.Length != 0){
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
        if(waveNumber < waves.Length)
            SettingWaveInChickenSpawn();
    }

    private void FixedUpdate()
    {
        SetTime();
        UpdateRankings();
    }

    private void SetTime()
    {
        if (time > 0)
            time -= Time.deltaTime;

        // if (time <= 170f && intensitySetting == 0)
        // {
        //     IncreaseIntensity("Chickens Incoming");
        // }
        // if (time <= 150f && intensitySetting == 1)
        // {
        //     IncreaseIntensity("Coop Cooperation");
        // }
        // if (time <= 120f && intensitySetting == 2)
        // {
        //     IncreaseIntensity("Flock Inbound");
        // }
        // if (time <= 100f && intensitySetting == 3)
        // {
        //     IncreaseIntensity("Chicken Horde");
        // }
        // if (time <= 60f && intensitySetting == 4)
        // {
        //     IncreaseIntensity("Poultry Panic");
        // }
        if (time <= 18f)
        {
            if(soundManager != null){
                soundManager.PlayLastSeconds();
                endSound = true;
            }
        }
        if (time <= 0)
        {
            gameOver = true;
            HandleResults();
        }
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
    }

    private void IncreaseIntensity(string speedUpText)
    {
        intensitySetting++;
        // chickenSpawn.UpdateIntensity(intensitySetting);
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
    public float roundTime;
    public String wavePrompt;
    public int standardChickenAmounts;
    public List<SpecialChicken> specialChickens;
}

[System.Serializable]
public class SpecialChicken
{
    public float timeToSpawn;
    public GameObject chicken;
    
}