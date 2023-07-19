using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI btnText;

    [Header("Level")]
    [SerializeField] public LevelInfoSO correspondingLevel;

    public void SetUI()
    {
        btnText.text = correspondingLevel.levelBtnText;
    }

    public void HandleClick()
    {
        FindObjectOfType<SceneFader>()?.FadeTo(correspondingLevel.gameLevelToLoad);
    }
}
