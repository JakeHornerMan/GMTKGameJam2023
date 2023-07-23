using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelInfoDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Image levelDifficultyColor;
    [SerializeField] private TextMeshProUGUI levelDifficultyText;

    [Header("Difficulty Color Indication Values")]
    [SerializeField] private Color easyColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color hardColor;

    private SceneFader sceneFader;

    private LevelInfoSO currentLevel;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetUI(LevelInfoSO levelInfo)
    {
        currentLevel = levelInfo;
        levelNameText.text = currentLevel.levelName;
        SetDifficultyUI();
        gameObject.SetActive(true);
    }

    private void SetDifficultyUI()
    {
        string difficultyString;
        Color difficultyColor;
        switch (currentLevel.levelDifficulty)
        {
            case LevelInfoSO.Difficulty.Easy:
                difficultyString = "Easy";
                difficultyColor = easyColor;
                break;
            case LevelInfoSO.Difficulty.Medium:
                difficultyString = "Medium";
                difficultyColor = mediumColor;
                break;
            case LevelInfoSO.Difficulty.Hard:
                difficultyString = "Hard";
                difficultyColor = hardColor;
                break;
            default:
                difficultyString = "Medium";
                difficultyColor = mediumColor;
                break;
        };

        levelDifficultyText.text = difficultyString;
        levelDifficultyColor.color = difficultyColor;
    }

    public void LoadSelectedLevel()
    {
        HideDisplay();
        sceneFader.FadeTo(currentLevel.gameLevelToLoad);
    }

    public void HideDisplay()
    {
        gameObject.SetActive(false);
    }
}
