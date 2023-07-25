using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    // Build LevelSelect Scene by Instantiating Level Buttons

    [Header("References")]
    [SerializeField] private Transform btnGrid;
    [SerializeField] private GameObject lvlBtnPrefab;

    private void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        if (WorldSelectUI.selectedWorld == null || WorldSelectUI.selectedWorld.worldLevels == null)
            return;

        foreach (LevelInfoSO level in WorldSelectUI.selectedWorld.worldLevels)
        {
            LevelButton newBtn = Instantiate(
                lvlBtnPrefab,
                btnGrid
            ).GetComponent<LevelButton>();

            newBtn.correspondingLevel = level;
            newBtn.SetUI();
        }
    }
}
