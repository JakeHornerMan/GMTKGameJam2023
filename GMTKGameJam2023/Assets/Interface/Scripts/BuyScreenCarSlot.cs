using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyScreenCarSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Color redColor = new (255 / 255f, 123 / 255f, 111 / 255f);
    [SerializeField] private Color greenColor = new (154 / 255f, 255 / 255f, 124 / 255f);

    public enum SlotType
    {
        Roster,
        CarShop,
        Scrapyard
    }

    public SlotType slotType;

    [SerializeField] private TextMeshProUGUI priceText;

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            BuyScreenCar car = null;
            BuyScreenUltimate ultimate = null;

            if (eventData.pointerDrag.TryGetComponent<BuyScreenCar>(out car) || eventData.pointerDrag.TryGetComponent<BuyScreenUltimate>(out ultimate)) //eventData.pointerDrag = The car being held by the mouse
            {
                //if (transform.childCount > 0)
                if ((transform.GetComponentInChildren<BuyScreenCar>() != null) || (transform.GetComponentInChildren<BuyScreenUltimate>() != null))
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
                                GameObject oldItem = eventData.pointerEnter.GetComponentInChildren<DragDrop>().gameObject;
                                BuyScreenManager.instance.RemoveFromShop(oldItem);
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
                    else if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == BuyScreenCarSlot.SlotType.Roster)
                    {
                        if (slotType == SlotType.Scrapyard)
                        {
                            if (ultimate != null)
                            {
                                eventData.pointerDrag.gameObject.transform.parent = transform;
                                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                                // Add Money
                                BuyScreenManager.instance.AddMoney(25);

                                //DESTROY THE VEHICLE
                                Destroy(eventData.pointerDrag.gameObject);
                            }
                            else if (car != null)
                            {
                                eventData.pointerDrag.gameObject.transform.parent = transform;
                                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                                // Add Money
                                if (car.correspondingCar.carShopPrice < 5)
                                {
                                    BuyScreenManager.instance.AddMoney(3);
                                }
                                else
                                {
                                    BuyScreenManager.instance.AddMoney(5);
                                }

                                //DESTROY THE VEHICLE
                                Destroy(eventData.pointerDrag.gameObject);
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
        BuyScreenCar car = transform.GetComponentInChildren<BuyScreenCar>();
        BuyScreenUltimate ultimate = transform.GetComponentInChildren<BuyScreenUltimate>();

        if (car != null)
        {
            priceText.text = "$" + car.correspondingCar.carShopPrice.ToString("0");
        }
        else if (ultimate != null)
        {
            priceText.text = "$" + ultimate.correspondingUltimate.ultimateShopPrice.ToString("0");
        }

        UpdateBGColour();


    }
}
