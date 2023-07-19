using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "World", fileName = "WorldConfig")]
public class WorldConfigSO : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] public Sprite worldBackground;
    [SerializeField] public string worldName = "The Motorway";

    [Header("World Levels")]
    [SerializeField] public LevelInfoSO[] worldLevels;
}
