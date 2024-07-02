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
    [SerializeField] private GameObject purchaseParticles;
    [SerializeField] private GameObject sellParticles;


    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        //tokenPriceText.text = correspondingCar.carShopPrice.ToString("0");
        if(correspondingCar != null)
            correspCarIcon.sprite = correspondingCar.GetComponent<ObjectInfo>().objectIcon;
    }

    public void EnablePurchaseParticles()
    {
        purchaseParticles.SetActive(true);
    }

    public void EnableSellParticles()
    {

        sellParticles.SetActive(true);

        sellParticles.transform.parent = BuyScreenManager.instance.gameObject.transform;

        //GameObject particles = Instantiate(sellParticles, transform.position, Quaternion.identity, BuyScreenManager.instance.gameObject.transform);
    }



}
