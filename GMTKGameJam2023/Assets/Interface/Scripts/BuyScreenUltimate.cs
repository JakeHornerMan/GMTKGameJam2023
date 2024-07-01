using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuyScreenUltimate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TextMeshProUGUI tokenPriceText;
    [SerializeField] private Image correspUltIcon;
    [SerializeField] private GameObject purchaseParticles;

    [Header("Car Values")]
    [SerializeField] public Ultimate correspondingUltimate;

    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        //tokenPriceText.text = correspondingUltimate.ultimateShopPrice.ToString("0");
        correspUltIcon.sprite = correspondingUltimate.GetComponent<ObjectInfo>().objectIcon;
    }

    public void EnableParticles()
    {
        purchaseParticles.SetActive(true);
    }
}
