using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI rankingText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI missedChickensText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private GameManager gameManager;

    [Header("Settings")]
    [SerializeField] private string missedChickensLabel = " Missed Chickens";
    [SerializeField] private string failureRanking = "You Failed";

    [Header("Audio")]
    [SerializeField] private AudioClip levelSuccessClip;
    [SerializeField] private AudioClip levelFailureClip;

    private SceneFader sceneFader;
    private Pause pause;
    private AudioSource audioSrc;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        audioSrc = FindObjectOfType<AudioSource>();
        pause = FindObjectOfType<Pause>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetUI();
        LevelEndAudio();
    }

    private void LevelEndAudio()
    {
        // TODO Check settings if Audio is enabled, and use that
        if (Points.currentRanking == failureRanking)
            audioSrc.PlayOneShot(levelFailureClip);
        else
            audioSrc.PlayOneShot(levelSuccessClip);
    }

    public void ReturnToMenu()
    {
        sceneFader.FadeToMainMenu();
    }

    public void Restart()
    {
        sceneFader.ReloadScene();
    }

    public void SetUI()
    {
        rankingText.text = Points.currentRanking;
        killsText.text = Points.killCount.ToString("000");
        missedChickensText.text = Points.safelyCrossedChickens.ToString("00") + " " + missedChickensLabel;
        finalScoreText.text = Points.playerScore.ToString("000");
        SetChickenWave();
    }

    public void SetChickenWave()
    {
        ChickenWave chickenWave = new ChickenWave();
        chickenWave.roundTime = 5f;
        chickenWave.standardChickenAmounts = Points.killCount;
        chickenWave.chickenIntesity = 5;
        chickenWave.coinAmount = Points.totalTokens;

        // SpecialChicken specialChicken = new SpecialChicken();
        chickenWave.specialChickens = null;

        gameManager.waves.Add(chickenWave);
        gameManager.SetStart();
    }
}