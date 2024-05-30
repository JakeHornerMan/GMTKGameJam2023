using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Part of Ultimate Select System
/// Contains corresponding Scriptable Object, along with UI References
/// Sets UI
/// When Clicked, Runs function on CardDraw.cs, passes its Scriptable Object
/// Has function to flip itself.
/// </summary>
public class UltimateSelectableCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI percentageTitleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image cardSprite; // Front or Back

    [Header("Sprites")]
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite frontSprite;

    [Header("Settings")]
    [SerializeField] public UltimateCardSO correspondingConfig;

    private void Start()
    {
        SetUI();
    }

    public void HandleClick()
    {
        FindObjectOfType<CardDraw>().HandleSelectedCard(this);
    }

    public void SetUI()
    {
        percentageTitleText.text = correspondingConfig.percentageString;
        descriptionText.text = correspondingConfig.descriptionString
            .Replace("{%}", correspondingConfig.percentageString);
    }

    public void FlipCard(bool front)
    {
        cardSprite.sprite = front ? frontSprite : backSprite;
    }

    public void SetUIActivation(bool active)
    {
        percentageTitleText.gameObject.SetActive(active);
        descriptionText.gameObject.SetActive(active);
    }
}
