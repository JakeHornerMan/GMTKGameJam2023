using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    

    public void EnterGame() => sceneFader.FadeToGame();
    public void EnterTutorial() => sceneFader.FadeToTutorial();
    public void EnterCredits() => sceneFader.FadeToCredits();

    public void QuitGame() => Application.Quit();
}
