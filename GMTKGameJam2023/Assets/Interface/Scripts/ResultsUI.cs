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

    [SerializeField] private string missedChickensLabel = " Missed Chickens";
    [SerializeField] private string pointsLabel = " Points";

    private SceneFader sceneFader;
    private Pause pause;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        pause = FindObjectOfType<Pause>();
        SetUI();
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
        finalScoreText.text = Points.playerScore.ToString("000") + " " + pointsLabel;
    }
}