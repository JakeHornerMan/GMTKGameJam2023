using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelPage : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI levelNameText;

    [HideInInspector] public LevelInfoSO selecedLevel;

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void SetUI()
    {
        levelNameText.text = selecedLevel.levelName;

        gameObject.SetActive(true);
    }

    public void PlayLevel()
    {
        sceneFader.FadeTo(selecedLevel.gameLevelToLoad);
    }
    
    public void ClosePage()
    {
        gameObject.SetActive(false);
    }
}
