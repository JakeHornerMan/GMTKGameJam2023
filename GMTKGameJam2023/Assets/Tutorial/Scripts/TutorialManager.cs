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

    private void SetPlayerValuesInLevel()
    {
        missedChickenLives = startLives;
    }

    private void FixedUpdate()
    {
        if (missedChickenLives <= 0)
        {
            if (tutroialRoundCounter < 8)
            {
                TutorialFail();
            }
            else
            {
                if (!tutorialOver) TutorialSuccess(false);
            }
        }
        if (!roundTimerStarted && chickenContainer.transform.childCount <= 0 && tokenContainer.transform.childCount <= 0 && !changingRound)
        {
            tutroialRoundCounter++;
            changingRound = true;
        }
        if (changingRound && !tutorialOver)
            TutorialRoundChanging();
        if (!roundOver)
        {
            SetTime();
        }
    }

    public void TutorialRoundChanging()
    {
        switch (tutroialRoundCounter)
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

    public void Round1LetChickenCrossIntroCornHealth()
    {
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

    public void Round2ChickenWaveAndStandardCar()
    {
        vehicleSpawner.disableVehicleSpawn = false;
        vehicleSpawner.SetStandardCar();
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        carCursorUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorWalletUI.transform.localScale = new Vector3(0, 0, 0);
        carCursorTokenUI.transform.localScale = new Vector3(0, 0, 0);
        OpenInfoBluePrintPauseGame("<b>Oh No!</b> <br><br>You let a chicken cross the road, and they've attacked your corn (<i>displayed above</i>). <br><br>To defend yourself, send cars down the road to hit the chickens and stop them from eating your crops! <br><br>We'll start with the default car for now, but you'll get the chance to buy other, more advanced vehicles later <br><br>(<i>Click a road to place them!</i>)");
    }

    public void Round3ChickenWaveAndCarWallet()
    {
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        vehicleSpawner.SetStandardCar();
        walletUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorWalletUI.transform.localScale = new Vector3(1f, 1f, 1f);
        OpenInfoBluePrintPauseGame("You can only place a limited amount of vehicles at one time. <br><br>Placing a vehicle uses a cog (<i>located on the left</i>), which regenerates every few seconds. <br><br>The time it takes to regenerate, as well as the number of cogs you have, are also shown on the game cursor.");
    }

    public void Round4TokenWave()
    {
        vehicleSpawner.disableVehicleSpawn = false;
        // tokenSpawner.SpawnToken(tokenPrefab, tokenContainer.transform);
        Instantiate(tokenPrefab, new Vector3(4f, 1f, 1f), Quaternion.identity, tokenContainer.transform);
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        vehicleSpawner.SetStandardCar();
        tokensUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carCursorTokenUI.transform.localScale = new Vector3(1f, 1f, 1f);
        OpenInfoBluePrintPauseGame("Make sure to collect blue energy tokens when you spot them by sending a car towards it!  <br><br>These can be used to send better vehicles which can hit more chickens- but you'll only get so many, so use them wisely!");
    }

    public void Round5NewVehiclesAndLanes()
    {
        vehicleSpawner.disableVehicleSpawn = false;
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        vehicleSpawner.disableVehicleSpawn = false;
        carsInLevel.Add(bonusCar);
        vehicleSpawner.DestroyButtons();
        vehicleSpawner.CreateButtons(carsInLevel);
        vehicleSpawner.SetStandardCar();
        tokensUI.transform.localScale = new Vector3(1f, 1f, 1f);
        carSelectorUI.transform.localScale = new Vector3(1f, 1f, 1f);
        OpenInfoBluePrintPauseGame("Now you have a new set of wheels! <br><br>Use them by clicking on a blueprint (<i>at the bottom of the screen</i>) or using the number keys (1, 2, 3, 4 etc), and then clicking on the lane you want to send it down. <br><br>You need to have the correct amount of energy to purchase a vehicle. <br><br>Different vehicles can be sent down different lanes, depending on their type.");
    }

    public void Round6SpecialChickens()
    {
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        OpenInfoBluePrintPauseGame("The Chickens are Evolving! <br><br> There are many different special types of chickens, each with unique mechanics designed to throw a wrench in your plans.");
    }

    public void Round7TopContainerShow()
    {
        vehicleSpawner.disableVehicleSpawn = false;
        topContainerUI.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        cornContainerUI.GetComponent<RectTransform>().anchoredPosition = originalCornContainerlocation;
        SetRoundTime();
        OpenInfoBluePrintPauseGame("You have just received an Ultimate ability. Use it by selecting the button (<i>located at the top</i>), or by pressing the space key, and click anywhere on the screen to place. <br><br>Also found above, on the left is the time remaining for the round. <br><br>On the right we have your total score, rack this up for bragging rights! <br><br> Pro tip: The more chickens you defeat with a single vehicle, the higher your combo.");
    }

    public void Round8IntenseWave()
    {
        SettingWaveInChickenSpawn(waves[tutroialRoundCounter - 1]);
        SetRoundTime();
        OpenInfoBluePrintPauseGame("Congratulations! <br><br>That's all you need to know to stop the chicken menace. <br><br>Now let's see if you can survive this round...");
    }

    public void TutorialFail()
    {
        missedChickenLives = startLives;
        healthCorn.ResetCorn();
        OpenInfoBluePrintPauseGame("Clucking hell! <br><br> You let too many chickens cross the road, pay close attension to your lives counter indicated by the corn icons. <br><br>We understand, you got curious as to why the chickens crossed the road, but this is serious business!  <br><br>We have reset your corn (lives), so get back out there and try again!");
    }

    public void TutorialSuccess(bool success)
    {
        tutorialOver = true;
        string text;
        if (success)
        {
            text = "You did it! Nice work. <br><br>";
        }
        else
        {
            text = "They got to the other side! <br><br>";
        }
        OpenInfoBluePrintPauseGame(text + "You completed the tutorial! <br><br>Good luck and have fun!", true);
    }

    public void SetRoundTime()
    {
        roundTimerStarted = true;
        time = waves[tutroialRoundCounter - 1].roundTime;
        roundOver = false;
    }

    private void SetPointsValuesInLevel()
    {
        safelyCrossedChickens = Points.safelyCrossedChickens;
        killCount = Points.killCount;
        totalTokens = Points.totalTokens;
        playerScore = Points.playerScore;
        interfaceManager.scoreForText = playerScore;
    }

    public void OpenInfoBluePrintPauseGame(string description, bool isOver = false)
    {
        Time.timeScale = 0;
        pause.isTutorialText = true;
        bluePrint.DisplayDescription(description);
        bluePrint.activateContinue(isOver);
    }

    public void CloseInfoBluePrintResumeGame()
    {
        // pause.UnpauseGame();
        Time.timeScale = 1;
        pause.isTutorialText = false;
        bluePrint.HandleClose();
    }

    public void ContinueToMainMenu()
    {
        tutorialOver = true;
        pause.UnpauseGame();
        sceneFader.transform.gameObject.SetActive(true);
        sceneFader.FadeToMainMenu();
    }

    // Starts a new chicken wave
    private void SettingWaveInChickenSpawn(ChickenWave wave)
    {
        ChickenWave currentWave = wave;
        NewWavePopup(currentWave.wavePrompt);
        Debug.Log("Standard Chickens in this round: " + currentWave.standardChickenAmounts);
        soundManager.PlayWaveSound(currentWave.waveSound);

        chickenSpawn.SetNewWave(currentWave);

        if (tokenSpawner != null)
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
        if (waveNumber < waves.Count)
        {
            // SettingWaveInChickenSpawn();
        }
    }

    //Increments time value for UI
    private void SetTime()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
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
        if (topContainerUI.transform.localScale != new Vector3(0, 0, 0))
            RemovePlayerScore(lostChickenScore * safelyCrossedChickens);
        soundManager.PlayMissedChicken();
        CameraShaker.instance.Shake(0.25f, -0.5f);
    }

    public void AddPlayerScore(int addAmount)
    {
        playerScore += addAmount;
        if (interfaceManager)
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
        sceneFader.FadeToBuyScreen();
    }

    private void HandleGameOver()
    {
        isGameOver = true;
        SetPointsValues();
        GameProgressionValues.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneFader.FadeToResults();
    }

    //Updates Static scripts
    private void SetPlayerValues()
    {
        PlayerValues.Cars = carsInLevel;
        PlayerValues.startingEnergy = 0;
    }

    private void SetPointsValues()
    {
        Points.killCount = killCount;
        Points.safelyCrossedChickens = safelyCrossedChickens;
        Points.playerScore = playerScore;
        Points.totalTokens = totalTokens;
        Debug.Log("playerScore: " + Points.playerScore);
    }
}
