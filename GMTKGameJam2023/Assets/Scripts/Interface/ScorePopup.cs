using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public void SetText(string msg)
    {
        popupText.text = msg;
    }
}
