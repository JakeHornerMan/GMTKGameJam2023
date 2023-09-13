using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject iconPrefab;

    [Header("Text Display Settings")]
    [SerializeField] private string timeUnit = "s";

    [Header("Colors")]
    [SerializeField] private Color easyColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color hardColor;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI lvlNumText;
    [SerializeField] private TextMeshProUGUI lvlDurationText;
    [SerializeField] private TextMeshProUGUI lvlNameText;
    [SerializeField] private TextMeshProUGUI lvlDifficultyText;
    [SerializeField] private Image lvlDifficultyImage;
    [SerializeField] private Transform carsInLevelContainer;
    [SerializeField] private Transform chickenInLevelContainer;

    [HideInInspector] public LevelInfoSO correspondingLevel;

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void SetUI()
    {
        lvlNumText.text = correspondingLevel.levelNum;
        lvlDurationText.text = correspondingLevel.gameDurationSeconds.ToString() + timeUnit;
        lvlNameText.text = correspondingLevel.levelName;
        lvlDifficultyText.text = correspondingLevel.levelDifficulty.ToString();

        lvlDifficultyImage.color = correspondingLevel.levelDifficulty switch
        {
            LevelInfoSO.Difficulty.Easy => easyColor,
            LevelInfoSO.Difficulty.Medium => mediumColor,
            LevelInfoSO.Difficulty.Hard => hardColor,
            _ => mediumColor,
        };

        foreach (Car car in correspondingLevel.carsInLevel)
        {
            LvlIcon newIcon = Instantiate(
                iconPrefab,
                carsInLevelContainer
            ).GetComponent<LvlIcon>();
            newIcon.SetImage(car.carIcon);
        }

        foreach (ChickenMovement chicken in correspondingLevel.chickensInLevel)
        {
            LvlIcon newIcon = Instantiate(
                iconPrefab,
                chickenInLevelContainer
            ).GetComponent<LvlIcon>();
            // newIcon.SetImage(chicken.chickenSpriteImage);
        }
    }

    // Function Run by Play Button
    public void LoadLevel()
    {
        sceneFader.FadeTo(correspondingLevel.gameLevelToLoad);
    }
}
