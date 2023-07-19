using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Level Configuration", fileName = "LevelConfig")]
public class LevelInfoSO : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] public string levelBtnText = "1";
    [SerializeField] public string levelName = "Level 1";

    [Header("Scene Loading")]
    [SerializeField] public string gameLevelToLoad = "Level_1";
}
