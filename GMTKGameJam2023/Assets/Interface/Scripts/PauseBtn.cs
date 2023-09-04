using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    private Pause pause;

    private void Awake()
    {
        pause = FindObjectOfType<Pause>();
    }

    public void HandleClick()
    {
        pause.PauseGame(true);
    }
}
