using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject carIconPrefab;
    [SerializeField] private GameObject chickenIconPrefab;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI lvlNumText;
    [SerializeField] private TextMeshProUGUI lvlDurationText;
    [SerializeField] private TextMeshProUGUI lvlNameText;
    [SerializeField] private TextMeshProUGUI lvlDifficultyText;
    [SerializeField] private Image lvlDifficultyImage;
    [SerializeField] private Transform carsInLevelContainer;
    [SerializeField] private Transform chickenInLevelContainer;

    [HideInInspector] public LevelInfoSO correspondingLevel;
}
