using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject confirmExitDialog;
    [SerializeField] private GameObject confirmRestartDialog;

    private SceneFader sceneFader;
    private Pause pause;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        pause = FindObjectOfType<Pause>();
    }

    private void Start()
    {
        confirmExitDialog.SetActive(false);
        confirmRestartDialog.SetActive(false);
    }

    public void ResumeGame() => pause.UnpauseGame();

    public void ReturnToMenu()
    {
        pause.UnpauseGame();
        SteamLeaderboards.InitAndUpdateScore(Points.playerScore, GameProgressionValues.RoundNumber);
        SaveGame.DeleteSaveFileAndStaticData();
        GameProgressionValues.SetDefaultValues();
        PlayerValues.SetDefaultValues();
        Points.SetDefaultValues();
        sceneFader.FadeToMainMenu();
    }

    public void Restart()
    {
        pause.UnpauseGame();
        GameProgressionValues.SetDefaultValues();
        PlayerValues.SetDefaultValues();
        Points.SetDefaultValues();
        sceneFader.ScreenWipeOut("Level01");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
