using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultsUI resultsUI;

    [Header("Player Stats")]
    public int safelyCrossedChickens = 0;
    public int killCount = 0;
    public int playerScore = 0;
    public int tokens = 0;
    public int totalTokens = 0;
    public float startTime = 120f;
    public int intesitySetting = 0;
    public string currentRanking = "Animal Lover";
    public bool gameOver = false;

    private SoundManager soundManager;
    private Pause pause;

    public float time = 120f;

    private void Awake()
    {
        pause = FindObjectOfType<Pause>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        tokens = 0;
        totalTokens = 0;

        time = startTime;
        resultsUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        SetTime();
    }

    private void SetTime()
    {
        time -= Time.deltaTime;
        if (time <= 110f && intesitySetting == 0)
        {
            intesitySetting ++;
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if (time <= 90f && intesitySetting == 1)
        {
            intesitySetting ++;
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if (time <= 60f && intesitySetting == 2)
        {
            intesitySetting ++;
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if (time <= 40f && intesitySetting == 3)
        {
            intesitySetting ++;
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if (time <= 0)
        {
            gameOver = true;
            soundManager.PlayEndMuisc();
            HandleResults();
        }
    }

    private void HandleResults()
    {
        //pause.PauseGame(showUI: false);
        resultsUI.SetUI(currentRanking, killCount, safelyCrossedChickens, totalTokens);
        resultsUI.gameObject.SetActive(true);
    }
}
