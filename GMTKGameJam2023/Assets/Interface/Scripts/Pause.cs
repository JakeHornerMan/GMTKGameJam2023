using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseUI;

    [Header("User Input")]
    [SerializeField] private KeyCode pauseKey = KeyCode.P;
    [SerializeField] private KeyCode pauseKeyAlt = KeyCode.Escape;

    [Header("Pause Values")]
    [SerializeField] private float pauseTimeScale = 0;

    private float startTimeScale = 1;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isTutorialText = false;

    private void Start()
    {
        startTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if(!isTutorialText){
            if (Input.GetKeyDown(pauseKey) || Input.GetKeyDown(pauseKeyAlt))
            {
                if (isPaused)
                    UnpauseGame();
                else
                    PauseGame();
            }
        }
    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = startTimeScale;
        pauseUI.SetActive(false);
    }

    public void PauseGame(bool showUI = true)
    {
        isPaused = true;
        Time.timeScale = pauseTimeScale;
        if (showUI)
            pauseUI.SetActive(true);
    }
}
