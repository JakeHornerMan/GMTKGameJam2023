using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    // Button Function
    // TODO Create LevelInfo scritpble object with level name, level time, level background image, etc. amd pass it in through the button
    public void LoadGameScene(string scene) => sceneFader.FadeTo(scene);
}
