using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyScreenCarSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    public enum SlotType
    {
        Roster,
        CarShop,
        Scrapyard
    }

    public SlotType slotType;

    private Color redColor = new Color(255/255f, 123/255f, 111/255f);
    private Color greenColor = new Color(154/255f, 255/255f, 124/255f);

    [SerializeField] private TextMeshProUGUI priceText;

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            BuyScreenCar car = null;
            BuyScreenUltimate ultimate = null;

            if (eventData.pointerDrag.TryGetComponent<BuyScreenCar>(out car) || eventData.pointerDrag.TryGetComponent<BuyScreenUltimate>(out ultimate)) //eventData.pointerDrag = The car being held by the mouse
            {
                if (transform.childCount > 0)
                {


                    if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == SlotType.CarShop) // If the car came from the shop
                    {
                        if ((car != null && BuyScreenManager.instance.CheckMoneyAmount(car.correspondingCar.carShopPrice)) || (ultimate != null && BuyScreenManager.instance.CheckMoneyAmount(ultimate.correspondingUltimate.ultimateShopPrice))) // If the player has enough money to buy the car
                        {
                            if (slotType == SlotType.Roster)
                            {
                                if (gameObject.name == "Ultimate Slot" && ultimate != null)
                                {
                                    eventData.pointerDrag.gameObject.transform.parent = transform;
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                                    // Take away any money
                                    BuyScreenManager.instance.RemoveMoney(ultimate.correspondingUltimate.ultimateShopPrice);
                                }
                                else if (gameObject.name != "Ultimate Slot" && car != null)
                                {
                                    eventData.pointerDrag.gameObject.transform.parent = transform;
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                                    // Take away any money
                                    if (car != null)
                                        BuyScreenManager.instance.RemoveMoney(car.correspondingCar.carShopPrice);
                                }

                                // Add pre-existing (pointerEnter) car to scrapyard
                                //GameObject oldItem = eventData.pointerEnter.GetComponentInChildren<DragDrop>().gameObject;
                                //BuyScreenManager.instance.AddToScrapyard(oldItem);
                            }
                        }

                    }
                    else
                    {
                        DragDrop childCar = eventData.pointerEnter.gameObject.GetComponentInChildren<DragDrop>();
                        //Takes the pre-existing item and moves it to where your new car used to be 
                        childCar.transform.SetParent(eventData.pointerDrag.GetComponent<DragDrop>().startingParent);
                        childCar.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                }
                else
                {
                    if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == BuyScreenCarSlot.SlotType.CarShop)
                    {
                        if ((car != null && BuyScreenManager.instance.CheckMoneyAmount(car.correspondingCar.carShopPrice)) || (ultimate != null && BuyScreenManager.instance.CheckMoneyAmount(ultimate.correspondingUltimate.ultimateShopPrice))) // If the player has enough money to buy the car
                        {

                            if (slotType == SlotType.Roster)
                            {
                                if (gameObject.name == "Ultimate Slot" && ultimate != null)
                                {
                                    eventData.pointerDrag.gameObject.transform.parent = transform;
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                                    // Take away any money
                                    BuyScreenManager.instance.RemoveMoney(ultimate.correspondingUltimate.ultimateShopPrice);
                                }
                                else if (gameObject.name != "Ultimate Slot" && car != null)
                                {
                                    eventData.pointerDrag.gameObject.transform.parent = transform;
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                                    // Take away any money
                                    if (car != null)
                                        BuyScreenManager.instance.RemoveMoney(car.correspondingCar.carShopPrice);
                                }
                                
                                    
                            }

                        }
                        else
                        {
                            eventData.pointerDrag.gameObject.transform.parent = eventData.pointerDrag.GetComponent<DragDrop>().startingParent;
                            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                            eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;
                        }

                    }


                }
            }
            
            




            //eventData.pointerDrag.gameObject.transform.parent = transform;
        }
        else
        {
            eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = false;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //When the player clicks on the item slot, the info box will be filled out with the corresponding information

        if (eventData.pointerClick != null)
        {
            GameObject shopItem = eventData.pointerDrag.gameObject;

            if (shopItem != null)
            {
                if (shopItem.TryGetComponent<BuyScreenCar>(out BuyScreenCar car))
                {
                    BuyScreenInfoBox.instance.FillInfoBoxCar(car);
                }
                else if (shopItem.TryGetComponent<BuyScreenUltimate>(out BuyScreenUltimate ultimate))
                {
                    BuyScreenInfoBox.instance.FillInfoBoxUltimate(ultimate);
                }
                
            }
        }
    }

    public void UpdateBGColour()
    {
        if (slotType == SlotType.CarShop)
        {
            if (transform.GetChild(0).TryGetComponent<BuyScreenCar>(out BuyScreenCar car))
            {
                if (BuyScreenManager.instance.CheckMoneyAmount(car.correspondingCar.carShopPrice)) //if the player has enough money to buy the car)
                {
                    priceText.color = greenColor;
                }
                else
                {
                    priceText.color = redColor;
                }
            }
            else if (transform.GetChild(0).TryGetComponent<BuyScreenUltimate>(out BuyScreenUltimate ultimate))
            {
                if (BuyScreenManager.instance.CheckMoneyAmount(ultimate.correspondingUltimate.ultimateShopPrice)) //if the player has enough money to buy the car)
                {
                    priceText.color = greenColor;
                }
                else
                {
                    priceText.color = redColor;
                }
            }
             
                
        }
    }

    public void UpdatePriceText()
    {
        if (transform.GetChild(0).TryGetComponent<BuyScreenCar>(out BuyScreenCar car))
        {
            priceText.text = car.correspondingCar.carShopPrice.ToString("0");
        }
        else if (transform.GetChild(0).TryGetComponent<BuyScreenUltimate>(out BuyScreenUltimate ultimate))
        {
            priceText.text = ultimate.correspondingUltimate.ultimateShopPrice.ToString("0");
        }
        
    }
}
