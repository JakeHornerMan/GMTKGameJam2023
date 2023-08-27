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
        sceneFader.FadeToMainMenu();
    }

    public void Restart()
    {
        pause.UnpauseGame();
        sceneFader.ReloadScene();
    }
}
