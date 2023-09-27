using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelect : MonoBehaviour
{
    public static WorldConfigSO selectedWorld;

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void LoadLevelSelect()
    {
        sceneFader.FadeToLevelSelect();
    }
}
