using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlIcon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image iconImg;

    public void SetImage(Sprite icon)
    {
        iconImg.sprite = icon;
    }
}
