using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    // Build LevelSelect Scene by Instantiating Level Buttons

    [Header("References")]
    [SerializeField] private Transform scrollingContainer;
    [SerializeField] private GameObject lvlCardPrefab;

    private void Start()
    {
        CreateLevelCards();
    }

    private void CreateLevelCards()
    {
        if (WorldSelect.selectedWorld == null) return;
        foreach (LevelInfoSO level in WorldSelect.selectedWorld.worldLevels)
        {
            LevelCard newLevelCard = Instantiate(
                lvlCardPrefab,
                scrollingContainer
            ).GetComponent<LevelCard>();
            newLevelCard.correspondingLevel = level;
        }
    }
}
