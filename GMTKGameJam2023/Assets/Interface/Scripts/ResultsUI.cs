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
    }

    public void ReturnToMenu()
    {
        pause.UnpauseGame();
        sceneFader.FadeToMainMenu();
    }

    public void Restart()
    {
        pause.UnpauseGame();
        sceneFader.ReloadScene();
    }

    public void SetUI(string ranking, int kills, int missedChickens, int finalScore)
    {
        rankingText.text = ranking;
        killsText.text = kills.ToString("000");
        missedChickensText.text = missedChickens.ToString("00") + " " + missedChickensLabel;
        finalScoreText.text = finalScore.ToString("000") + " " + pointsLabel;
    }
}