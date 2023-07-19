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

    public void EnterWorldSelect() => sceneFader.FadeToWorlds();
    public void EnterTutorial() => sceneFader.FadeToTutorial();
    public void EnterCredits() => sceneFader.FadeToCredits();
    public void EnterSettings() => sceneFader.FadeToSettings();

    public void QuitGame() => Application.Quit();
}
