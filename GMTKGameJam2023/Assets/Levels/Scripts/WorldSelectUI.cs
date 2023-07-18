using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldSelectUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI selectedWorldName;
    [SerializeField] private Image selectedWorldBackground;

    [Header("Worlds")]
    [SerializeField] private WorldConfigSO[] worlds;

    private int selectedWorldIndex;
    private WorldConfigSO selectedWorld;

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    private void Start()
    {
        selectedWorldIndex = 0;
        selectedWorld = worlds[selectedWorldIndex];
    }

    private void Update()
    {
        selectedWorld = worlds[selectedWorldIndex];
        SetUI(selectedWorld);
    }

    private void SetUI(WorldConfigSO selectedWorld)
    {
        selectedWorldName.text = selectedWorld.worldName;
        selectedWorldBackground.sprite = selectedWorld.worldBackground;
    }

    // Button Functions
    public void Scroll(bool increase)
    {
        if (increase)
            selectedWorldIndex++;
        else
            selectedWorldIndex--;
        selectedWorldIndex = Mathf.Clamp(selectedWorldIndex, 0, worlds.Length - 1);
    }

    public void LoadWorldLevelSelect()
    {
        sceneFader.FadeTo(selectedWorld.levelSelectSceneToLoad);
    }
}
