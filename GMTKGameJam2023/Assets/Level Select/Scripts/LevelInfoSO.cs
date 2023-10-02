using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Level Configuration", fileName = "LevelConfig")]
public class LevelInfoSO : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] public Sprite levelArt;
    [SerializeField] public string levelNum = "1";
    [SerializeField] public string levelName = "Level 1";
    [SerializeField] public Difficulty levelDifficulty = Difficulty.Easy;

    [Header("Cars Info")]
    [SerializeField] public ObjectInfo[] carsInLevel;

    [Header("Chicken Types")]
    [SerializeField] public ObjectInfo[] chickensInLevel;

    [Header("Waves Info")]
    [SerializeField] public float gameDurationSeconds = 100;
    [SerializeField] public float numWaves = 5;

    [Header("Scene Loading")]
    [SerializeField] public string gameLevelToLoad = "Level_1";

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
