using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldSelectUI : MonoBehaviour
{
    // Choose Selected World and Assign to Static Value

    [Header("References")]
    [SerializeField] private TextMeshProUGUI selectedWorldName;
    [SerializeField] private Image selectedWorldBackground;

    [Header("Worlds")]
    [SerializeField] private WorldConfigSO[] worlds;

    private int selectedWorldIndex;
    public static WorldConfigSO selectedWorld;

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
        sceneFader.FadeToLevelSelect();
    }
}
