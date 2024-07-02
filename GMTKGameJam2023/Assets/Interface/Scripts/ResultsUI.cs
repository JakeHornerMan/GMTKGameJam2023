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
        GameProgressionValues.SetDefaultValues();
        PlayerValues.SetDefaultValues();
        Points.SetDefaultValues();
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
    }

    public string SetRankText(){
        if (GameProgressionValues.RoundNumber >= 30)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = gold;
            return "1. Master Chicken Assassin";
        }
        else if (GameProgressionValues.RoundNumber >= 25)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = gold;
            return "2. The Colonel";
        }
        else if (GameProgressionValues.RoundNumber >= 20)
        {
            goldTrophy.enabled = false;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = gold;
            return "3. The Cluck Slayer";
        }
        else if (GameProgressionValues.RoundNumber >= 15)
        {
            goldTrophy.enabled = true;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = silver;
            return "4. The Eggs-terminator";
        }
        else if (GameProgressionValues.RoundNumber >= 10)
        {
            goldTrophy.enabled = true;
            silverTrophy.enabled = false;
            bronzeTrophy.enabled = false;
            rankingText.color = silver;
            return "5. Average Fast Food worker";
        }
        else if (GameProgressionValues.RoundNumber >= 5)
        {
            goldTrophy.enabled = true;
            silverTrophy.enabled = true;
            bronzeTrophy.enabled = false;
            rankingText.color = bronze;
            return "6. Chicken Lover";
        }
        else if (GameProgressionValues.RoundNumber < 5)
        {
            goldTrophy.enabled = true;
            silverTrophy.enabled = true;
            bronzeTrophy.enabled = false;
            rankingText.color = bronze;
            return "7. Animals Crossing";
        }


        return "Meh...";
    }
}