using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelInfoSO correspondingLevel;

    public void HandleClick()
    {
        FindObjectOfType<SceneFader>()?.FadeTo(correspondingLevel.gameLevelToLoad);
    }
}
