using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        SetStaticValuesToDefault();
        gameManager = FindObjectOfType<GameManager>();
        SetUI();
        LevelEndAudio();
    }

    private void LevelEndAudio()
    {
        audioSrc.PlayOneShot(levelSuccessClip);
    }

    public void ReturnToMenu()
    {
        sceneFader.FadeToMainMenu();
    }

    public void Restart()
    {
        // sceneFader.RestartLevel();
        sceneFader.ScreenWipeOut("Level01");
    }

    public void SetUI()
    {
        // TODO: RankingRequirement based off points
        rankingText.text = "Meh";
        killsText.text = Points.killCount.ToString("000");
        missedChickensText.text = Points.safelyCrossedChickens.ToString("00") + " " + missedChickensLabel;
        finalScoreText.text = Points.playerScore.ToString();
        SetChickenWave();
    }

    public void SetChickenWave()
    {
        ChickenWave chickenWave = new ChickenWave();
        chickenWave.roundTime = 5f;
        chickenWave.standardChickenAmounts = Points.safelyCrossedChickens;
        chickenWave.chickenIntesity = 5;
        chickenWave.coinAmount = Points.totalTokens;
        chickenWave.specialChickens = null;

        gameManager.waves.Add(chickenWave);
        // gameManager.SetStart();
    }

    private void SetStaticValuesToDefault(){
        GameProgressionValues.SetDefaultValues();
    }
}