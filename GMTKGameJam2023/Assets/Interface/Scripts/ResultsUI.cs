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
    }

    private void Start()
    {
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
    }
}