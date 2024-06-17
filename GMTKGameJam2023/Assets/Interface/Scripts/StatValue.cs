using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;

public class StatValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statValue;

    public void SetValues(StatResultValue value)
    {
        statValue.text = value.Title + " : " + value.Score.ToString();
    }
}
