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
    [SerializeField] private TextMeshProUGUI totalTokensText;

    [SerializeField] private string missedChickensLabel = "Missed Chickens";

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

    public void SetUI(string ranking, int kills, int missedChickens, int totalTokens)
    {
        rankingText.text = ranking;
        killsText.text = kills.ToString("000");
        missedChickensText.text = missedChickens.ToString("00") + missedChickensLabel;
        totalTokensText.text = totalTokens.ToString("000");
    }
}