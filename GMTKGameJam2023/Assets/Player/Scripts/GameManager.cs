using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Developer Settings")]
    [SerializeField] public float startTime = 180f;
    [SerializeField] public bool devMode = false;
    [SerializeField] public bool isGameOver = false;

    [Header("Cars in Level")]
    [SerializeField] public Car[] carsInLevel;

    [Header("Gameplay Settings")]
    [SerializeField] public int startLives = 10;
    [SerializeField] public int lostChicenScore = 1000;

    [Header("Player Stats (Don't Modify in Inspector)")]
    public int safelyCrossedChickens = 0;
    public int missedChickenLives = 0;
    public int killCount = 0;
    public int playerScore = 0;
    public int tokens = 0;
    public int totalTokens = 0;
    public string currentRanking = "Animal Lover";

    [Header("Current Time (Don't Modify in Inspector)")]
    public float time = 120f;

    [Header("Other Values (Don't Modify in Inspector)")]
    public bool endSound = false;
    public int waveNumber = 0;

    [Header("ChickenWaves")]
    [SerializeField] public List<ChickenWave> waves;

    private SoundManager soundManager;
    private Pause pause;
    private ChickenSpawn chickenSpawn;
    private SceneFader sceneFader;
    private InterfaceManager interfaceManager;
    private CameraShaker cameraShaker;

    public delegate void EventType();
    public static event EventType OnTokensUpdated;

    //[Header("Events")]

    

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
        missedChickenLives = startLives;
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        //tokens = 0;
        totalTokens = 0;

        SetGameTime();
        time = startTime;

        if (waves.Count != 0)
            SettingWaveInChickenSpawn();

        if (isGameOver)
            MissedChickensWave();

    }

    private void SetGameTime()
    {
        float gameTime = 0f;
        if (waves.Count != 0)
        {
            foreach (ChickenWave value in waves)
            {
                gameTime += value.roundTime;
            }
            startTime = gameTime;
        }
    }

    private void SettingWaveInChickenSpawn()
    {
        // Debug.Log("Current Wave: "+ waveNumber);

        ChickenWave currentWave = waves[waveNumber];
        NewWavePopup(currentWave.wavePrompt);

        chickenSpawn.SetNewWave(currentWave);

        IEnumerator coroutine = WaitAndNextWave(currentWave.roundTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndNextWave(float time)
    {
        yield return new WaitForSeconds(time);
        waveNumber++;
        if (waveNumber < waves.Count)
            SettingWaveInChickenSpawn();
    }

    private void MissedChickensWave()
    {
        ChickenWave endWave = new()
        {
            roundTime = 10f,
            standardChickenAmounts = Points.safelyCrossedChickens,
            wavePrompt = "",
            specialChickens = new List<SpecialChicken>()
        };
        waves.Add(endWave);
        SettingWaveInChickenSpawn();
    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            SetTime();
        }
        // UpdateRankings();
    }

    private void SetTime()
    {
        if (time > 0)
            time -= Time.deltaTime;

        if (time <= 0 || missedChickenLives <= 0)
        {
            isGameOver = true;
            UpdateRankings();
            HandleResults();
        }
        if (time <= 18f)
        {
            if (soundManager != null)
            {
                soundManager.PlayLastSeconds();
                endSound = true;
            }
        }
    }

    public void SafelyCrossedChicken()
    {
        safelyCrossedChickens++;
        missedChickenLives--;
        RemovePlayerScore(lostChicenScore * safelyCrossedChickens);
        soundManager.PlayMissedChicken();
        StartCoroutine(cameraShaker.Shake(0.25f, -0.5f));
    }

    public void AddPlayerScore(int addAmount)
    {
        interfaceManager.ScoreUI(addAmount, true);
        playerScore += addAmount;
    }

    public void RemovePlayerScore(int removeAmount)
    {
        // interfaceManager.ScoreUI(removeAmount, false);
        playerScore -= removeAmount;
        playerScore = Mathf.Clamp(playerScore, 0, playerScore);
    }

    public void AddTokens(int addAmount)
    {
        // interfaceManager.TokenUI(addAmount, true);
        tokens += addAmount;
        totalTokens += addAmount;
    }

    public void RemoveTokens(int removeAmount)
    {
        interfaceManager.TokenUI(removeAmount, false);
        tokens -= removeAmount;
    }

    public void UpdateTokens(int tokenDifference)
    {
        tokens = tokens + tokenDifference;

        if (tokenDifference > 0)
        {
            totalTokens = totalTokens + tokenDifference;
        }
        

        OnTokensUpdated();
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

        if (missedChickenLives <= 0)
        {
            currentRanking = "You Failed";
        }
    }

    private void NewWavePopup(string speedUpText)
    {
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
    public ChickenWave() { }
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