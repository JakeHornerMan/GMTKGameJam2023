using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuyScreenCar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TextMeshProUGUI tokenPriceText;
    [SerializeField] private Image correspCarIcon;


    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        tokenPriceText.text = correspondingCar.carShopPrice.ToString("0");
        correspCarIcon.sprite = correspondingCar.GetComponent<ObjectInfo>().objectIcon;
    }

}
