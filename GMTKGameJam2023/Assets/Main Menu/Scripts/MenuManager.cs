using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Tags")]
    [SerializeField] private string musicTag = "Music";
    private AudioSource musicAudioSource;

    [Header("References")]
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject beginBackButton;
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private RoundSkipPopup roundSkipPopup;

    public bool showRoundSkipPopupBeforeLoad = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SaveGame.LoadTheGame();
        musicAudioSource = GameObject.FindGameObjectWithTag(musicTag).GetComponent<AudioSource>();
        roundSkipPopup = FindObjectOfType<RoundSkipPopup>();
    }

    public void ShowPlayBackButton()
    {
        backButton.SetActive(true);
        backButton.GetComponent<Animator>().Play("ShowBackButton");
    }

    public void HidePlayBackButton()
    {
        backButton.GetComponent<Animator>().Play("HideBackButton");
    }

    public void ShowBeginBackButton()
    {
        beginBackButton.SetActive(true);
        beginBackButton.GetComponent<Animator>().Play("ShowBackButton");
    }

    public void HideBeginBackButton()
    {
        beginBackButton.GetComponent<Animator>().Play("HideBackButton");
    }

    // Function for regular scene transitions (from TriggerMenuCinemachine)
    public void EnterScene(string sceneName)
    {
        StartCoroutine(NormalWipeAndLoad(sceneName));
    }

    // When Begin Clicked, Determine whether or not to do a popup
    public void EnterGameScene()
    {
        // bool unlockedCheckpoint = true;
        bool unlockedCheckpoint = TopRound.topRound >= 10;
        bool hasSavedGame = true; // PLACEHOLDER boolean, determines whether save will show or not

        if (SaveGame.DoesSaveFileExist())
        {
            StartCoroutine(WipeAndLoadGameWithSavedRound());
        }
        else if (unlockedCheckpoint)
        {
            StartCoroutine(WipeAndLoadGameWithCheckpoint());
        }
        else
        {
            StartCoroutine(NormalWipeAndLoad("Level01"));
        }
    }

    // Handle start from round 1 or 5 When player clicked begin 
    private IEnumerator WipeAndLoadGameWithCheckpoint()
    {
        string sceneName;

        // Reset Game Progression Values
        GameProgressionValues.SetDefaultValues();
        Points.SetDefaultValues();
        yield return new WaitForSeconds(2.5f);

        // Open Round Skip Popup and wait for user response
        bool playerChoseToSkip = false;
        yield return StartCoroutine(WaitForRoundSkipResponse((response) => playerChoseToSkip = response));

        // Chose to skip to checkpoint
        if (playerChoseToSkip)
        {
            PlayerValues.SetRound5Values();
            // Set Gameprogression Values here for that chosen round
            GameProgressionValues.SetRound5Values();
            sceneName = "BuyScreenImproved";
        }
        // "Start from first round" chosen
        else sceneName = "Level01";

        SaveGame.DeleteSaveFileAndStaticData();

        // Wipe out to level 1
        sceneFader.ScreenWipeOut(sceneName);
    }

    // See if player wants to continue save or start new game
    private IEnumerator WipeAndLoadGameWithSavedRound()
    {
        // SaveGame.SetGameDataForGame();
        string sceneName = "BuyScreenImproved";

        // Reset Game Progression Values
        GameProgressionValues.SetDefaultValues();
        Points.SetDefaultValues();
        yield return new WaitForSeconds(2.5f);

        // Open Popup and wait for response
        bool playerChoseToResumeSave = false;
        yield return StartCoroutine(WaitForSavedRoundResponse((response) => playerChoseToResumeSave = response));

        // Chose to skip to checkpoint
        if (playerChoseToResumeSave)
        {
            SaveGame.SetGameDataForGame();
            if(SaveGame.DoesSaveFileExist()){
                // if(SaveGame.saveDataLoaded == null){
                //     // SaveGame.saveDataLoaded.SetValues();
                //     SaveGame.SetGameDataForGame();
                // }
                // else{
                //     SaveGame.LoadTheGame();
                //     SaveGame.SetGameDataForGame();
                // }
            }
            SaveGame.isLoadingASave = true;
            // PLACEHOLDER: SET VALUES FOR SAVED GAME
            sceneName = "BuyScreenImproved";
            // Wipe out to saved Game
            sceneFader.ScreenWipeOut(sceneName);
        }
        // Player does not want to resume save, so ask them about Checkpoint
        else
        {
            GameProgressionValues.SetDefaultValues();
            PlayerValues.SetDefaultValues();
            Points.SetDefaultValues();
            // bool unlockedCheckpoint = true; // DEBUG THING IF DEV HAS NOT REACHED ROUND 10
            bool unlockedCheckpoint = TopRound.topRound >= 10;
            if (unlockedCheckpoint)
            {
                roundSkipPopup.ShowLoadingIndicator();
                StartCoroutine(WipeAndLoadGameWithCheckpoint());
            }
            else
            {
                roundSkipPopup.ShowLoadingIndicator();
                StartCoroutine(NormalWipeAndLoad("Level01"));
            }
            roundSkipPopup.HideSavegamePopupUI();
        }
    }

    // Open popup UI and Wait for player to choose either skip to checkpoint or start from round 1
    private IEnumerator WaitForRoundSkipResponse(System.Action<bool> callback)
    {
        bool responseReceived = false;
        bool playerChoseToSkip = false;

        // Open popup UI
        roundSkipPopup.OpenPopupUI(5); // Replace 5 with the actual round number

        ShowBeginBackButton();

        // Subscribe to button click events
        roundSkipPopup.skipRoundButton.onClick.AddListener(() =>
        {
            playerChoseToSkip = true;
            responseReceived = true;
        });

        roundSkipPopup.roundOneButton.onClick.AddListener(() =>
        {
            playerChoseToSkip = false;
            responseReceived = true;
        });

        // Wait until a response is received
        while (!responseReceived)
        {
            yield return null;
        }

        // Unsubscribe from button click events
        roundSkipPopup.skipRoundButton.onClick.RemoveAllListeners();
        roundSkipPopup.roundOneButton.onClick.RemoveAllListeners();

        // Execute callback with the player's choice
        callback(playerChoseToSkip);
    }

    // Open popup UI and Wait for player to choose either resume saved game or start new game
    private IEnumerator WaitForSavedRoundResponse(System.Action<bool> callback)
    {
        bool responseReceived = false;
        bool playerChoseToResumeSave = false;

        // Open popup UI
        roundSkipPopup.OpenSavedGamePopupUI();

        ShowBeginBackButton();

        // Subscribe to button click events
        roundSkipPopup.resumeSavedGameButton.onClick.AddListener(() =>
        {
            playerChoseToResumeSave = true;
            responseReceived = true;
        });

        roundSkipPopup.newGameButton.onClick.AddListener(() =>
        {
            playerChoseToResumeSave = false;
            responseReceived = true;
        });

        // Wait until a response is received
        while (!responseReceived)
        {
            yield return null;
        }

        // Unsubscribe from button click events
        roundSkipPopup.resumeSavedGameButton.onClick.RemoveAllListeners();
        roundSkipPopup.newGameButton.onClick.RemoveAllListeners();

        // Execute callback with the player's choice
        callback(playerChoseToResumeSave);
    }

    // Normal Scene wipe transition
    private IEnumerator NormalWipeAndLoad(string sceneName)
    {
        // Reset Game Progression Values
        GameProgressionValues.SetDefaultValues();
        Points.SetDefaultValues();
        yield return new WaitForSeconds(2.5f);

        sceneFader.ScreenWipeOut(sceneName);
    }
}
