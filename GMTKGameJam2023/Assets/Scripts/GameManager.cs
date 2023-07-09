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
    public int intensitySetting = 0;
    public string currentRanking = "Animal Lover";
    public bool gameOver = false;

    private SoundManager soundManager;
    private Pause pause;
    private ChickenSpawn chickenSpawn;
    private InterfaceManager interfaceManager;

    public float time = 120f;

    private void Awake()
    {
        pause = FindObjectOfType<Pause>();
        soundManager = FindObjectOfType<SoundManager>();
        chickenSpawn = GetComponent<ChickenSpawn>();
        interfaceManager = GetComponent<InterfaceManager>();
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
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        
        if (time <= 170f && intensitySetting == 0)
        {
            intensitySetting ++;
            chickenSpawn.UpdateIntensity(intensitySetting);
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
            interfaceManager.ShowSpeedUpText("Chickens Incoming!");
        }
        if (time <= 150f && intensitySetting == 1)
        {
            intensitySetting ++;
            chickenSpawn.UpdateIntensity(intensitySetting);
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
            interfaceManager.ShowSpeedUpText("Coop Cooperation");
        }
        if (time <= 120f && intensitySetting == 2)
        {
            intensitySetting ++;
            chickenSpawn.UpdateIntensity(intensitySetting);
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
            interfaceManager.ShowSpeedUpText("Flock Inbound");
        }
        if (time <= 100f && intensitySetting == 3)
        {
            intensitySetting ++;
            chickenSpawn.UpdateIntensity(intensitySetting);
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
            interfaceManager.ShowSpeedUpText("Chicken Horde");
        }
        if (time <= 60f && intensitySetting == 4)
        {
            intensitySetting ++;
            chickenSpawn.UpdateIntensity(intensitySetting);
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
            interfaceManager.ShowSpeedUpText("Poultry Panic");
        }
        if (time <= 18f)
        {
            soundManager.PlaySound(SoundManager.SoundType.LastSeconds);
            interfaceManager.ShowSpeedUpText("Time Nearly Up");
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
        resultsUI.SetUI(currentRanking, killCount, safelyCrossedChickens, playerScore);
        resultsUI.gameObject.SetActive(true);
    }
}
