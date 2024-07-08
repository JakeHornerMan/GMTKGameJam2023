using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private SceneFader sceneFader;
    private Pause pause;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        pause = FindObjectOfType<Pause>();
    }

    public void ResumeGame() => pause.UnpauseGame();

    public void ReturnToMenu()
    {
        pause.UnpauseGame();
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
