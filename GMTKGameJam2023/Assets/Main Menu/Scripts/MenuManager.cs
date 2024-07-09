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
    [SerializeField] private SceneFader sceneFader;

    private RoundSkipPopup roundSkipPopup;
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
        musicAudioSource = GameObject.FindGameObjectWithTag(musicTag).GetComponent<AudioSource>();
        roundSkipPopup = FindObjectOfType<RoundSkipPopup>();
    }

    public void ShowBackButton()
    {
        backButton.SetActive(true);
        backButton.GetComponent<Animator>().Play("ShowBackButton");
    }

    public void HideBackButton()
    {
        backButton.GetComponent<Animator>().Play("HideBackButton");
    }

    // Function for regular scene transitions (from TriggerMenuCinemachine)
    public void EnterScene(string sceneName)
    {
        StartCoroutine(WipeAndLoadGame(sceneName));
    }

    // Run Coroutine with a parameter telling it to do main menu round skip popup
    public void EnterMainMenuScene()
    {
        StartCoroutine(WipeAndLoadGame("", loadingGameScene: true));
    }

    // Loading game scene is set to true only when clicking begin to start the game.
    private IEnumerator WipeAndLoadGame(string sceneName, bool loadingGameScene = false)
    {
        // Reset Game Progression Values
        GameProgressionValues.SetDefaultValues();
        Points.SetDefaultValues();
        yield return new WaitForSeconds(2.5f);

        // When player clicked begin
        if (loadingGameScene)
        {
            // Open Round Skip Popup and wait for user response
            bool playerChoseToSkip = false;
            yield return StartCoroutine(WaitForRoundSkipResponse((response) => playerChoseToSkip = response));

            // Chose to skip to checkpoint
            if (playerChoseToSkip)
            {
                // Set Gameprogression Values here for that chosen round
                sceneName = "ProceduralGeneration";
            }
            // "Start from first round" chosen
            else sceneName = "Level01";

            // Wipe out to level 1
            sceneFader.ScreenWipeOut(sceneName);
        }
        // Normal Scene Wipe (not for play game)
        else
        {
            sceneFader.ScreenWipeOut(sceneName);
        }
    }

    private IEnumerator WaitForRoundSkipResponse(System.Action<bool> callback)
    {
        bool responseReceived = false;
        bool playerChoseToSkip = false;

        // Open popup UI
        roundSkipPopup.OpenPopupUI(5); // Replace 5 with the actual round number

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
}
