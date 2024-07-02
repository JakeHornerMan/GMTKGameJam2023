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
    [SerializeField] private GameObject sellParticles;

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
