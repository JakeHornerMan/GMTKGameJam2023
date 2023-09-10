using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavToWorldSelect : MonoBehaviour
{
    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void ReturnToWorldSelect()
    {
        sceneFader.FadeToWorlds();
    }
}
