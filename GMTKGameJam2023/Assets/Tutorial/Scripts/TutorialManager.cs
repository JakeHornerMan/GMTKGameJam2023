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
    [HideInInspector] public bool roundOver = true;
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
    [SerializeField] private ObjectBlueprint bluePrint;
    private bool roundTimerStarted = false;
    public int tutroialRoundCounter = 1;
    private bool changingRound = true;
    private bool tutorialOver = false;
    private VehicleSpawner vehicleSpawner;
    [SerializeField] private GameObject tokenPrefab; 
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

        // if(tutroialRoundCounter != 1){
        //     sceneFader.transform.gameObject.SetActive(false);
        // }
    }

    private void SetPlayerValuesInLevel(){
        missedChickenLives = startLives;
    }

    private void FixedUpdate()
    {   
        if(missedChickenLives <= 0){
            if(tutroialRoundCounter < 8){
                TutorialFail();
            }
            else{
                if(!tutorialOver) TutorialSuccess(false);
            }
        }
        if(!roundTimerStarted && chickenContainer.transform.childCount <= 0 && tokenContainer.transform.childCount <= 0 && !changingRound){
            tutroialRoundCounter++;
            changingRound = true;
        }
        if(changingRound && !tutorialOver)
            TutorialRoundChanging();
        if(!roundOver){
            SetTime();
        } 
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
            Round5NewVehiclesAndLanes();
            changingRound = false;
            break;
        case 6:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round6SpecialChickens();
            changingRound = false;
            break;
        case 7:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round7TopContainerShow();
            changingRound = false;
            break;
        case 8:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            Round8IntenseWave();
            changingRound = false;
            break;
        case 9:
            Debug.Log("Tutorial Round" + tutroialRoundCounter);
            TutorialSuccess(true);
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
        OpenInfoBluePrintPauseGame("<b>Oh No!</b> <br><br>You have let a chicken cross the road, and attack your corn <i>(displayed on top)</i>. <br><br>You can now send standard car to stop them from eating your crops <br><br><i>(Please select a road to place them!)</i>");
    }

    public void Round3ChickenWaveAndCarWallet(){
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        vehicleSpawner.setStandardCar();
        walletUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorWalletUI.transform.localScale = new Vector3(1f, 1f, 1f);
        OpenInfoBluePrintPauseGame("You can only place a limited amount of vehicles at one time. <br><br>This is shown by the cogs on the left side of the screen, or the number and spinner attached to the game cursor. <br><br>These vehicles are recharged every few seconds to keep you fighting the chicken menace.");
    }

    public void Round4TokenWave(){
        vehicleSpawner.disableVehicleSpawn = false;
        tokenSpawner.SpawnToken(tokenPrefab, tokenContainer.transform);
        // Instantiate(tokenPrefabs, new Vector3(1f, 1f, 1f),Quaternion.identity, tokenContainer.transform);
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        vehicleSpawner.setStandardCar();
        tokensUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorTokenUI.transform.localScale = new Vector3(1f, 1f, 1f);
        OpenInfoBluePrintPauseGame("Energy tokens are a very important resourse to collect! <br><br>They show up randomly during your playtime and are collected by vehicles. <br><br>Soon you will be able to able to afford a new set of wheels");
    }

    public void Round5NewVehiclesAndLanes(){
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        vehicleSpawner.disableVehicleSpawn = false;
        carsInLevel.Add(bonusCar);
        vehicleSpawner.DestroyButtons();
        vehicleSpawner.CreateButtons(carsInLevel);
        vehicleSpawner.setStandardCar();
        tokensUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carSelectorUI.transform.localScale = new Vector3(1f, 1f, 1f);
        OpenInfoBluePrintPauseGame("Now you have a new set of wheels! <br><br>You can now select the new bluprint icons <i>(on the bottom of the screen)</i>. Place them on different roads that highlight when selected. <br><br>You need to have the corrrect amount of energy to purchase a vehicle");
    }

    public void Round6SpecialChickens(){
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        OpenInfoBluePrintPauseGame("The Chickens are Evolving! <br><br> There is many different special types of chickens with different mechanics to throw a wrench in your plans.");
    }

    public void Round7TopContainerShow(){
        vehicleSpawner.disableVehicleSpawn = false;
        topContainerUI.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        cornContainerUI.GetComponent<RectTransform>().anchoredPosition = originalCornContainerlocation;
        SetRoundTime();
        OpenInfoBluePrintPauseGame("You have just recieved a Ultimate ability. Select the ability <i>(top center button)</i> and click any where on the screen to place. <br><br> On the left, you have timer for the round counting down until the next chicken horde. <br><br>On the right we have your player score, rack this up for bragging rights");
    }

    public void Round8IntenseWave(){
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter-1]);
        SetRoundTime();
        OpenInfoBluePrintPauseGame("Congratulations! <br><br>You have completed the tuttorial. <br><br>Now lets see if you can survive this round.");
    }

    public void TutorialFail(){
        missedChickenLives = startLives;
        healthCorn.ResetCorn();
        OpenInfoBluePrintPauseGame("Clucking hell! <br><br> You let too many chickens cross the road, pay close attension to your lives counter with the corn icons. <br><br>It's ok you were curious why the chickens crossed the road.  <br><br>We have reset your corn (lives) to complete the tutorial!");   
    }

    public void TutorialSuccess(bool success){
        tutorialOver = true;
        string text;
        if(success){
            text = "You did it! <br><br>";
        }
        else{
            text = "They got to the other side! <br><br>";
        }
        OpenInfoBluePrintPauseGame(text + "You completed the tutorial. <br><br> Now lets see what you are made off.", true);   
    }

    public void SetRoundTime(){
        roundTimerStarted = true;
        time = waves[tutroialRoundCounter-1].roundTime;
        roundOver = false;
    }

    private void SetPointsValuesInLevel(){
        safelyCrossedChickens = Points.safelyCrossedChickens;
        killCount = Points.killCount;
        totalTokens = Points.totalTokens;
        playerScore =  Points.playerScore;
        interfaceManager.scoreForText = playerScore;
    }

    public void OpenInfoBluePrintPauseGame(string description, bool isOver = false)
    {
        pause.PauseGame(false);
        pause.isTutorialText = true;
        bluePrint.DisplayDescription(description);
        bluePrint.activateContinue(isOver);
    }

    public void CloseInfoBluePrintResumeGame()
    {
        pause.UnpauseGame();
        pause.isTutorialText = false;
        bluePrint.HandleClose();
    }

    public void ContinueToMainMenu()
    {
        tutorialOver = true;
        pause.UnpauseGame();
        sceneFader.transform.gameObject.SetActive(true);
        sceneFader.ScreenWipeOut("MainMenuInteractive Jack Ver");
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
        if (time > 0){
            time -= Time.deltaTime;
        }
        else{
            roundOver = true;
            tutroialRoundCounter++;
            changingRound = true;
        }
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
        sceneFader.ScreenWipeOut("BuyScreenImproved");
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
