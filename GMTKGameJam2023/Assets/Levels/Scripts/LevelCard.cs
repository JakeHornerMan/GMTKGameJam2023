using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject overflowCountPrefab;

    [Header("Icon Display Settings")]
    [SerializeField] private int maxRowIcons = 4;
    [Tooltip("Number of icons to show on card. If more, will go to an overflow menu.")]

    [Header("Text Display Settings")]
    [SerializeField] private string timeUnit = "s";

    [Header("Colors")]
    [SerializeField] private Color easyColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color hardColor;

    [Header("UI References")]
    [SerializeField] private Image levelArt;
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
        levelArt.sprite = correspondingLevel.levelArt;

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

        CreateIcons(correspondingLevel.carsInLevel, carsInLevelContainer);
        CreateIcons(correspondingLevel.chickensInLevel, chickenInLevelContainer);
    }

    private void CreateIcons(ObjectInfo[] objects, Transform container)
    {
        // Keep Track of Icons Placed
        int iconsPlaced = 0;

        foreach (ObjectInfo obj in objects)
        {
            // Handle Overflow, spawn overflow button
            if (objects.Length > maxRowIcons && iconsPlaced == maxRowIcons - 1)
            {
                IconOverflow overflowIcon = Instantiate(
                    overflowCountPrefab,
                    container
                ).GetComponent<IconOverflow>();

                // Give the overflow button all the extra icons not displayed
                for (int i = maxRowIcons-1; i < objects.Length; i++)
                    overflowIcon.overflowObjects.Add(objects[i]);

                break;
            }
            else
            {
                // Create Icons Normally
                LvlIcon newIcon = Instantiate(
                    iconPrefab,
                    container
                ).GetComponent<LvlIcon>();
                newIcon.SetImage(obj.objectIcon);
                iconsPlaced++;
            }
        }
    }

    // Function Run by Play Button
    public void LoadLevel()
    {
        sceneFader.FadeTo(correspondingLevel.gameLevelToLoad);
    }
}
