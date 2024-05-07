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
    [SerializeField] public GameObject tokenContainer;

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

    [Header("Tutorial References")]
    public int tutroialRoundCounter = 1;
    private bool changingRound = true;
    private VehicleSpawner vehicleSpawner;
    [SerializeField] private Car bonusCar;
    [SerializeField] private GameObject topContainerUI;
    [SerializeField] private GameObject tokensUI;
    [SerializeField] private GameObject carSelectorUI;
    [SerializeField] private GameObject walletUI;
    [SerializeField] private GameObject carCursorUI;
    [SerializeField] private GameObject carCursorTokenUI;
    [SerializeField] private GameObject carCursorWalletUI;

    [SerializeField] private GameObject cornContainerUI;
    private Vector3 originalCornContainerlocation;

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
        healthCorn = GetComponent<HealthCorn>();
        vehicleSpawner = GetComponent<VehicleSpawner>();

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
            // SettingWaveInChickenSpawn();
        // }
        // else{
        //     RoundSet();
        // }  
        Round1LetChickenCrossIntroCornHealth();
    }

    private void SetPlayerValuesInLevel(){
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
            missedChickenLives = cheatLives;
    //     }
    }

    private void FixedUpdate()
    {   
        if(chickenContainer.transform.childCount <= 0 && tokenContainer.transform.childCount <= 0 && !changingRound){
            tutroialRoundCounter++;
            changingRound = true;
        }
        if(changingRound)
            TutorialRoundChanging();

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

    public void TutorialRoundChanging(){
        switch(tutroialRoundCounter) 
        {
            case 1:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round1LetChickenCrossIntroCornHealth();
            changingRound = false;
            break;
        case 2:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round2ChickenWaveAndStandardCar();
            changingRound = false;
            break;
        case 3:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round3ChickenWaveAndCarWallet();
            changingRound = false;
            break;
        case 4:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round4TokenWave();
            changingRound = false;
            break;
        case 5:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round5NewVehicle();
            changingRound = false;
            break;
        case 6:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round6NewVehicleNewRoad();
            changingRound = false;
            break;
        case 7:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round7SpecialChickens();
            changingRound = false;
            break;
        case 8:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round8TopContainerShow();
            changingRound = false;
            break;
        default:
            // code block
            break;
        }
    }

    public void Round1LetChickenCrossIntroCornHealth(){
        //disable visuals fo elements
        topContainerUI.transform.localScale = new Vector3(0, 0, 0);
        tokensUI.transform.localScale = new Vector3(0, 0, 0);
        carSelectorUI.transform.localScale = new Vector3(0, 0, 0);
        walletUI.transform.localScale = new Vector3(0, 0, 0);
        carCursorUI.transform.localScale = new Vector3(0, 0, 0);

        originalCornContainerlocation = cornContainerUI.GetComponent<RectTransform>().anchoredPosition;
        cornContainerUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 450f, 0f);
        vehicleSpawner.disableVehicleSpawn = true;
    }

    public void Round2ChickenWaveAndStandardCar(){
        vehicleSpawner.disableVehicleSpawn = false;
        vehicleSpawner.setStandardCar();
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        carCursorUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorWalletUI.transform.localScale = new Vector3(0, 0, 0);
        carCursorTokenUI.transform.localScale = new Vector3(0, 0, 0);
    }

    public void Round3ChickenWaveAndCarWallet(){
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        vehicleSpawner.setStandardCar();
        walletUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorWalletUI.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Round4TokenWave(){
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        vehicleSpawner.setStandardCar();
        tokensUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorTokenUI.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Round5NewVehicle(){
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        vehicleSpawner.setStandardCar();
        tokensUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carSelectorUI.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Round6NewVehicleNewRoad(){
        vehicleSpawner.disableVehicleSpawn = false;
        carsInLevel.Add(bonusCar);
        vehicleSpawner.DestroyButtons();
        vehicleSpawner.CreateButtons(carsInLevel);
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
    }

    public void Round7SpecialChickens(){
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
    }

    public void Round8TopContainerShow(){
        vehicleSpawner.disableVehicleSpawn = false;
        topContainerUI.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        cornContainerUI.GetComponent<RectTransform>().anchoredPosition = originalCornContainerlocation;
    }

    public void Round9IntenseWave(){
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
    }

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
        // SettingWaveInChickenSpawn();
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
    private void SettingWaveInChickenSpawn(ChickenWave wave)
    {
        ChickenWave currentWave = wave;
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
            // SettingWaveInChickenSpawn();
        }    
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
        if(topContainerUI.transform.localScale != new Vector3(0, 0, 0))
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

        // OnTokensUpdated();
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
