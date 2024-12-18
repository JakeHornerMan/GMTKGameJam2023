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
    [HideInInspector] public bool cancelPause = false;

    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource sfxAudio;

    private void Start()
    {
        startTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(pauseKey) || Input.GetKeyDown(pauseKeyAlt)) && FindObjectOfType<CardDraw>() == null)
        {
            if (isPaused)
                UnpauseGame();
            else
                PauseGame();
        }
    }

    public void UnpauseGame()
    {
        isPaused = false;
        if (FindObjectOfType<CardDraw>() == null)
            Time.timeScale = startTimeScale;
        if (FindObjectOfType<ObjectBlueprint>() == null)
            Time.timeScale = startTimeScale;

        if (musicAudio != null) musicAudio.UnPause();
        sfxAudio.UnPause();
        pauseUI.SetActive(false);
    }

    public void PauseGame(bool showUI = true)
    {
        if(!cancelPause){
            isPaused = true;
            Time.timeScale = pauseTimeScale;
            if (musicAudio != null) musicAudio.Pause();
            sfxAudio.Pause();
            if (FindObjectOfType<CardDraw>() != null)
                sfxAudio.UnPause();
            if (showUI)
                pauseUI.SetActive(true);
        }
    }
}
