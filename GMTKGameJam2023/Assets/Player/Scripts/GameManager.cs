using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Chicken Waves")]
    [SerializeField] public List<ChickenWave> waves;

    [Header("Cars in Level")]
    [SerializeField] public List<Car> carsInLevel;
    // [SerializeField] public Car[] carsInLevel;
    
    [SerializeField] public List<Car> defaultCars;
    [SerializeField] public Ultimate ultimateInLevel;

    [Header("Ranking Criteria, order highest to lowest")]
    [SerializeField] private RankingRequirement[] rankingCriteria;
    [SerializeField] private string failureRanking = "You Failed";

    [Header("Developer Settings")]
    [SerializeField] public bool isGameOver = false;
    [SerializeField] public bool pauseGameplay = false;
    [SerializeField] public float startTime = 180f;
    [SerializeField] public bool devMode = false;
    [SerializeField] private int cheatTokenAmount = 500;
    [SerializeField] private int cheatLives = 10;

    [Header("Gameplay Settings")]
    [SerializeField] public int startLives = 10;
    [SerializeField] public int lostChickenScore = 1000;

    // Player Stats
    [HideInInspector] public int safelyCrossedChickens = 0;
    [HideInInspector] public int missedChickenLives = 0;
    [HideInInspector] public int killCount = 0;
    [HideInInspector] public int playerScore = 0;
    [HideInInspector] public int tokens = 0;
    [HideInInspector] public int totalTokens = 0;
    [HideInInspector] public string currentRanking = "Animal Lover";

    // Current Time
    [HideInInspector] public float time = 120f;

    // Other Values
    [HideInInspector] public bool endSound = false;
    [HideInInspector] public int waveNumber = 0;
    [HideInInspector] public bool roundOver = false;
    [SerializeField] public GameObject chickenContainer;

    private SoundManager soundManager;
    private GameFlowManager gameFlowManager;
    private Pause pause;
    private ChickenSpawn chickenSpawn;
    private TokenSpawner tokenSpawner;
    public SceneFader sceneFader;
    private InterfaceManager interfaceManager;
    private CameraShaker cameraShaker;

    [HideInInspector] public delegate void EventType();
    [HideInInspector] public static event EventType OnTokensUpdated;

    private void Awake()
    {
        SetPlayerValuesInLevel();
        pause = FindObjectOfType<Pause>();
        soundManager = FindObjectOfType<SoundManager>();
        chickenSpawn = GetComponent<ChickenSpawn>();
        tokenSpawner = FindObjectOfType<TokenSpawner>();
        interfaceManager = GetComponent<InterfaceManager>();
        sceneFader = FindObjectOfType<SceneFader>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameFlowManager = FindObjectOfType<GameFlowManager>();
        chickenContainer = GameObject.Find("ChickenContainer");
    }

    private void Start()
    {
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        totalTokens = 0;

        if (devMode)
            tokens = cheatTokenAmount;
        if (waves.Count != 0 && devMode){
            SetGameTime();
            time = startTime;
            waveNumber = 0;
            SettingWaveInChickenSpawn();
        }
        else{
            RoundSet();
        }  
    }

    private void SetPlayerValuesInLevel(){
        if(!devMode){
            if(PlayerValues.Cars != null){
                carsInLevel = PlayerValues.Cars;
            }
            else{
                carsInLevel = defaultCars;
            }
            if(PlayerValues.ultimate != null){
                ultimateInLevel = PlayerValues.ultimate;
            }
            else{
                ultimateInLevel = null;
            }
            // Debug.Log("Cars: " + carsInLevel);
            missedChickenLives = PlayerValues.missedChickenLives;
            // Debug.Log("Lives: " + missedChickenLives);
        }
        else {
            missedChickenLives = cheatLives;
        }
    }

    private void RoundSet(){
        waves.Clear();
        if(gameFlowManager)
            gameFlowManager.newRound();
        SetGameTime();
        time = startTime;
        waveNumber = 0;
        SettingWaveInChickenSpawn();
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
        ChickenWave currentWave = waves[waveNumber];
        NewWavePopup(currentWave.wavePrompt);
        Debug.Log("Standard Chickens in this round: " + currentWave.standardChickenAmounts);
        soundManager.PlayWaveSound(currentWave.waveSound);

        chickenSpawn.SetNewWave(currentWave);

        if(tokenSpawner != null)
            tokenSpawner.SetNewWave(currentWave);

        IEnumerator coroutine = WaitAndNextWave(currentWave.roundTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndNextWave(float time)
    {
        yield return new WaitForSeconds(time);
        waveNumber++;
        if (waveNumber < waves.Count){
            SettingWaveInChickenSpawn();
        }    
        // else{
        //     RoundSet();
        // }
    }
    
    private void FixedUpdate()// !!!HERE WE CONTROL NEXT LEVEL AND GAMEOVER!!!
    {
        if (!isGameOver)
        {
            if(!pauseGameplay)
                SetTime();
        }
        if(missedChickenLives <= 0){
            HandleGameOver();
        }
        if(roundOver){
            SetPlayerValues();
            StartCoroutine(WaitAndBuyScreen(1f));
        }
    }

    private void SetPlayerValues(){
        PlayerValues.Cars = carsInLevel;
    }

    private void SetTime()
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
            roundOver = true;
    }

    private IEnumerator WaitAndBuyScreen(float time)
    {
        //sceneFader.Fade();
        Points.playerScore += playerScore;
        yield return new WaitForSeconds(time);
        sceneFader.ScreenWipeOut("BuyScreen");
    }

    public void SafelyCrossedChicken()
    {
        safelyCrossedChickens++;
        missedChickenLives--;
        RemovePlayerScore(lostChickenScore * safelyCrossedChickens);
        soundManager.PlayMissedChicken();
        CameraShaker.instance.Shake(0.25f, -0.5f);
    }

    public void AddPlayerScore(int addAmount)
    {
        if(interfaceManager)
            interfaceManager.ScoreUI(addAmount, true);
        playerScore += addAmount;
    }

    public void RemovePlayerScore(int removeAmount)
    {
        playerScore -= removeAmount;
        playerScore = Mathf.Clamp(playerScore, 0, playerScore);
        interfaceManager.ScoreUI(removeAmount, false);
    }

    public void AddTokens(int addAmount)
    {
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

    private void HandleGameOver(){
        isGameOver = true;
        UpdateRankings();
        HandleResults();
        ResetGameProgressionValues();
    }

    private void ResetGameProgressionValues()
    {

    }

    private void UpdateRankings()
    {
        // Sort the ranking criteria array in descending order by minKills
        Array.Sort(rankingCriteria, (a, b) => b.minScore.CompareTo(a.minScore));

        // Update Rankings
        foreach (var requirement in rankingCriteria)
        {
            if (playerScore > requirement.minScore)
            {
                currentRanking = requirement.rankingString;
                break;
            }
        }

        if (missedChickenLives <= 0)
        {
            currentRanking = failureRanking;
        }
    }

    private void NewWavePopup(string speedUpText)
    {
        soundManager?.PlayGameSpeed();
        interfaceManager?.ShowSpeedUpText(speedUpText);
    }

    private void HandleResults()
    {
        Points.currentRanking = currentRanking;
        Points.killCount = killCount;
        Points.safelyCrossedChickens = safelyCrossedChickens;
        Points.playerScore += playerScore;
        Debug.Log("Score: " + playerScore);
        Debug.Log("playerScore: " + Points.playerScore);
        Points.totalTokens = totalTokens;
        GameProgressionValues.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(sceneFader.WipeToScene("Results"));
    }
}
