using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBtn : MonoBehaviour
{
    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void ReturnToMainMenu()
    {
        sceneFader.FadeToMainMenu();
    }
}
