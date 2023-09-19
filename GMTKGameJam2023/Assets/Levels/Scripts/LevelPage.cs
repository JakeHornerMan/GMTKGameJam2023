using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelPage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectIconPrefab;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Transform levelCarsContainer;
    [SerializeField] private Transform levelChickenContainer;

    [HideInInspector] public LevelInfoSO selecedLevel;

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void SetUI()
    {
        levelNameText.text = selecedLevel.levelName;

        CreateIcons(selecedLevel.carsInLevel, levelCarsContainer);
        CreateIcons(selecedLevel.chickensInLevel, levelChickenContainer);

        gameObject.SetActive(true);
    }

    private void CreateIcons(ObjectInfo[] objects, Transform container)
    {
        foreach (ObjectInfo obj in objects)
        {
            // Create Icons Normally
            LvlIcon newIcon = Instantiate(
                objectIconPrefab,
                container
            ).GetComponent<LvlIcon>();
            newIcon.SetImage(obj.objectIcon);
        }
    }

    public void PlayLevel()
    {
        sceneFader.FadeTo(selecedLevel.gameLevelToLoad);
    }
    
    public void ClosePage()
    {
        gameObject.SetActive(false);

        // Clear out UI
        foreach (Transform obj in levelCarsContainer.transform)
            Destroy(obj.gameObject);
        foreach (Transform obj in levelChickenContainer.transform)
            Destroy(obj.gameObject);
    }
}
