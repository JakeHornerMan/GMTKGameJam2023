using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
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
    private HealthCorn healthCorn;

    [HideInInspector] public delegate void EventType();
    [HideInInspector] public static event EventType OnTokensUpdated;

    private void Awake()
    {
        // SetPlayerValuesInLevel();
        pause = FindObjectOfType<Pause>();
        soundManager = FindObjectOfType<SoundManager>();
        chickenSpawn = GetComponent<ChickenSpawn>();
        tokenSpawner = FindObjectOfType<TokenSpawner>();
        interfaceManager = GetComponent<InterfaceManager>();
        sceneFader = FindObjectOfType<SceneFader>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameFlowManager = FindObjectOfType<GameFlowManager>();
        healthCorn = GetComponent<HealthCorn>();

        chickenContainer = GameObject.Find("ChickenContainer");
    }

    private void Start()
    {
        // // SetPointsValuesInLevel();
        // if (devMode)
        //     tokens = cheatTokenAmount;
        // if (waves.Count != 0 && devMode){
        //     SetGameTime();
        //     time = startTime;
        //     waveNumber = 0;
            SettingWaveInChickenSpawn();
        // }
        // else{
        //     RoundSet();
        // }  
    }

    // private void SetPlayerValuesInLevel(){
    //     if(!devMode){
    //         if(PlayerValues.Cars != null){
    //             carsInLevel = PlayerValues.Cars;
    //         }
    //         else{
    //             carsInLevel = defaultCars;
    //         }
    //         if(PlayerValues.ultimate != null){
    //             ultimateInLevel = PlayerValues.ultimate;
    //         }
    //         else{
    //             ultimateInLevel = null;
    //         }
    //         // Debug.Log("Cars: " + carsInLevel);
    //         missedChickenLives = PlayerValues.missedChickenLives;
    //         tokens = PlayerValues.startingEnergy;

    //         // Debug.Log("Lives: " + missedChickenLives);
    //     }
    //     else {
    //         missedChickenLives = cheatLives;
    //     }
    // }

    private void SetPointsValuesInLevel(){
        safelyCrossedChickens = Points.safelyCrossedChickens;
        killCount = Points.killCount;
        totalTokens = Points.totalTokens;
        playerScore =  Points.playerScore;
        interfaceManager.scoreForText = playerScore;
    }

    // Settings For the Levels Round
    private void RoundSet(){
        waves.Clear();
        if(gameFlowManager)
            gameFlowManager.newRound();
        SetGameTime();
        time = startTime;
        waveNumber = 0;
        SettingWaveInChickenSpawn();
    }

    //set level round time
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

    // Starts a new chicken wave
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

    private void NewWavePopup(string speedUpText)
    {
        soundManager?.PlayGameSpeed();
        interfaceManager?.ShowSpeedUpText(speedUpText);
    }

    // Waits until current wave is over and starts the next one
    private IEnumerator WaitAndNextWave(float time)
    {
        yield return new WaitForSeconds(time);
        waveNumber++;
        if (waveNumber < waves.Count){
            SettingWaveInChickenSpawn();
        }    
    }
    
    // HERE WE CONTROL NEXT LEVEL AND GAMEOVER
    private void FixedUpdate()
    {
        // if (!isGameOver)
        // {
        //     if(!pauseGameplay)
        //         // SetTime();
        // }
        // if(missedChickenLives <= 0){
        //     HandleGameOver();
        // }
        // if(roundOver){
        //     StartCoroutine(WaitAndBuyScreen(3f));
        // }
    }

    //Increments time value for UI
    private void SetTime()
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
            roundOver = true;
    }

    //Health loss for player when chicken safely crosses
    public void SafelyCrossedChicken()
    {
        safelyCrossedChickens++;
        missedChickenLives--;
        healthCorn.DeadCorn(missedChickenLives);
        RemovePlayerScore(lostChickenScore * safelyCrossedChickens);
        soundManager.PlayMissedChicken();
        CameraShaker.instance.Shake(0.25f, -0.5f);
    }

    public void AddPlayerScore(int addAmount)
    {
        playerScore += addAmount;
        if(interfaceManager)
            interfaceManager.ScoreUI(addAmount, true);
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

    private IEnumerator WaitAndBuyScreen(float time)
    {
        sceneFader.Fade();
        SetPlayerValues();
        SetPointsValues();
        Points.playerScore = playerScore;
        Debug.Log("playerScore: " + Points.playerScore);
        yield return new WaitForSeconds(time);
        sceneFader.ScreenWipeOut("BuyScreen");
    }

    private void HandleGameOver(){
        isGameOver = true;
        SetPointsValues();
        GameProgressionValues.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneFader.FadeToResults();
    }

    //Updates Static scripts
    private void SetPlayerValues(){
        PlayerValues.Cars = carsInLevel;
        PlayerValues.startingEnergy =0;
    }

    private void SetPointsValues(){
        Points.killCount = killCount;
        Points.safelyCrossedChickens = safelyCrossedChickens;
        Points.playerScore = playerScore;
        Points.totalTokens = totalTokens;
        Debug.Log("playerScore: " + Points.playerScore);
    }
}
