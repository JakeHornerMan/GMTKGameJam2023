using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI rankingText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI missedChickensText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI roundText;
    private GameManager gameManager;

    [Header("Settings")]
    [SerializeField] private string missedChickensLabel = " Missed Chickens";
    [SerializeField] private string failureRanking = "You Failed";

    [SerializeField] private Color gold;
    [SerializeField] private Image goldTrophy;
    [SerializeField] private Color silver;
    [SerializeField] private Image silverTrophy;
    [SerializeField] private Color bronze;
    [SerializeField] private Image bronzeTrophy;

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
        audioSrc.PlayOneShot(levelSuccessClip);
    }

    public void ReturnToMenu()
    {
        sceneFader.FadeToMainMenu();
    }

    public void Restart()
    {
        GameProgressionValues.SetDefaultValues();
        PlayerValues.SetDefaultValues();
        Points.SetDefaultValues();
        sceneFader.ScreenWipeOut("Level01");
    }

    public void SetUI()
    {
        // TODO: RankingRequirement based off points
        rankingText.text = SetRankText();
        killsText.text = Points.killCount.ToString("000");
        missedChickensText.text = Points.safelyCrossedChickens.ToString("00") + " " + missedChickensLabel;
        finalScoreText.text = Points.playerScore.ToString();
        roundText.text = "Round "+ GameProgressionValues.RoundNumber.ToString("00");
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

    public string SetRankText(){
        if (GameProgressionValues.RoundNumber >= 50)
        {
            rankingText.color = gold;
            goldTrophy.enabled = true;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            return "1. Master Chicken Assassin";
        }
        else if (GameProgressionValues.RoundNumber >= 40)
        {
            goldTrophy.enabled = true;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = gold;
            return "2. KFC Manager";
        }
        else if (GameProgressionValues.RoundNumber >= 30)
        {
            goldTrophy.enabled = true;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = gold;
            return "3. KFC Worker";
        }
        else if (GameProgressionValues.RoundNumber >= 20)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = true;
            bronzeTrophy.enabled = false;
            rankingText.color = silver;
            return "4. Vehicularly Sus";
        }
        else if (GameProgressionValues.RoundNumber >= 20)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = true;
            bronzeTrophy.enabled = false;
            rankingText.color = silver;
            return "5. Accidents Happen";
        }
        else if (GameProgressionValues.RoundNumber >= 10)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = true;
            rankingText.color = bronze;
            return "6. Traffic Obeyer";
        }
        else if (GameProgressionValues.RoundNumber < 10)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = true;
            rankingText.color = bronze;
            return "7. Chicken Lover";
        }


        return "Meh...";
    }
}