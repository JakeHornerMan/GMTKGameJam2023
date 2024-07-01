using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyScreenCarSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Color redColor = new(255 / 255f, 123 / 255f, 111 / 255f);
    [SerializeField] private Color greenColor = new(154 / 255f, 255 / 255f, 124 / 255f);

    public enum SlotType
    {
        Roster,
        CarShop,
        Scrapyard
    }

    public SlotType slotType;

    [SerializeField] private TextMeshProUGUI priceText;

    public void OnDrop(PointerEventData eventData)
    {
        FindObjectOfType<BuyScreenManager>().itemPurchased = true;
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            BuyScreenCar car = null;
            BuyScreenUltimate ultimate = null;

            if (eventData.pointerDrag.TryGetComponent<BuyScreenCar>(out car) || eventData.pointerDrag.TryGetComponent<BuyScreenUltimate>(out ultimate)) //eventData.pointerDrag = The car being held by the mouse
            {
                //if (transform.childCount > 0)
                if ((transform.GetComponentInChildren<BuyScreenCar>() != null) || (transform.GetComponentInChildren<BuyScreenUltimate>() != null)) //If the slot is not empty
                {


                    if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == SlotType.CarShop) // If the car came from the shop
                    {
                        if ((car != null && BuyScreenManager.instance.CheckMoneyAmount(car.correspondingCar.carShopPrice)) || (ultimate != null && BuyScreenManager.instance.CheckMoneyAmount(ultimate.correspondingUltimate.ultimateShopPrice))) // If the player has enough money to buy the car
                        {
                            if (slotType == SlotType.Roster)
                            {
                                if (gameObject.name == "Ultimate Slot" && ultimate != null)
                                {
                                    ultimate.gameObject.transform.parent = transform;
                                    ultimate.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    ultimate.GetComponent<DragDrop>().canBePlaced = true;
                                    ultimate.GetComponent<DragDrop>().startingParent = transform;

                                    // Take away any money
                                    BuyScreenManager.instance.RemoveMoney(ultimate.correspondingUltimate.ultimateShopPrice);

                                    ultimate.EnableParticles();
                                }
                                else if (gameObject.name != "Ultimate Slot" && car != null)
                                {
                                    car.gameObject.transform.parent = transform;
                                    car.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    car.GetComponent<DragDrop>().canBePlaced = true;
                                    car.GetComponent<DragDrop>().startingParent = transform;

                                    // Take away any money
                                    BuyScreenManager.instance.RemoveMoney(car.correspondingCar.carShopPrice);
                                    car.EnableParticles();



                                }

                                // Sells the vehicle being replaced
                                GameObject oldItem = eventData.pointerEnter.GetComponentInChildren<DragDrop>().gameObject;
                                SellVehicle(oldItem);
                            }
                        }

                    }
                    else
                    {
                        if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == SlotType.Roster && eventData.pointerEnter.GetComponentInChildren<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == SlotType.Roster) // If both cars are currently in the roster
                        {

                            DragDrop heldCar = eventData.pointerDrag.gameObject.GetComponentInChildren<DragDrop>();
                            DragDrop replacedCar = eventData.pointerEnter.gameObject.GetComponentInChildren<DragDrop>();

                            GameObject oldParent = heldCar.startingParent.gameObject;
                            GameObject newParent = replacedCar.startingParent.gameObject;

                            //Takes the pre-existing item and moves it to where your new car used to be 
                            replacedCar.transform.SetParent(oldParent.transform);
                            replacedCar.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                            //Takes your held car and moves it to where the old car used to be
                            heldCar.transform.SetParent(gameObject.transform);
                            heldCar.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                            heldCar.GetComponent<DragDrop>().canBePlaced = true;

                        }

                    }
                }
                else
                {
                    if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == SlotType.CarShop)
                    {
                        if ((car != null && BuyScreenManager.instance.CheckMoneyAmount(car.correspondingCar.carShopPrice)) || (ultimate != null && BuyScreenManager.instance.CheckMoneyAmount(ultimate.correspondingUltimate.ultimateShopPrice))) // If the player has enough money to buy the car
                        {

                            if (slotType == SlotType.Roster)
                            {
                                if (gameObject.name == "Ultimate Slot" && ultimate != null)
                                {
                                    ultimate.gameObject.transform.parent = transform;
                                    ultimate.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    ultimate.GetComponent<DragDrop>().canBePlaced = true;
                                    ultimate.GetComponent<DragDrop>().startingParent = transform;


                                    // Take away any money
                                    BuyScreenManager.instance.RemoveMoney(ultimate.correspondingUltimate.ultimateShopPrice);

                                    ultimate.EnableParticles();
                                }
                                else if (gameObject.name != "Ultimate Slot" && car != null)
                                {
                                    car.gameObject.transform.parent = transform;
                                    car.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                                    car.GetComponent<DragDrop>().canBePlaced = true;
                                    car.GetComponent<DragDrop>().startingParent = transform;

                                    // Take away any money
                                    if (car != null)
                                    {
                                        BuyScreenManager.instance.RemoveMoney(car.correspondingCar.carShopPrice);
                                        car.EnableParticles();
                                    }
                                }

                                // NEW: HIDE THE LABEL TEXT (Car/Ult)
                                GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
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
                            SellVehicle(eventData.pointerDrag);
                        }
                        else if (slotType == SlotType.Roster)
                        {
                            eventData.pointerDrag.gameObject.transform.parent = transform;
                            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                            eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;
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
            GameObject shopItem = null;

            // Check if pointerDrag is not null before accessing its gameObject
            if (eventData.pointerDrag != null)
            {
                shopItem = eventData.pointerDrag.gameObject;
            }

            if (shopItem != null)
            {
                // Check if it's a right-click (secondary button)
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    Button button = shopItem.transform.parent.GetComponentInChildren<Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke(); // Invoke the button's onClick event
                    }
                }
            }
        }
    }

    void SellVehicle(GameObject vehicle)
    {

        BuyScreenCar car = null;
        BuyScreenUltimate ultimate = null;


        if (vehicle.TryGetComponent<BuyScreenCar>(out car) || vehicle.TryGetComponent<BuyScreenUltimate>(out ultimate))
        {
            if (ultimate != null)
            {
                ultimate.gameObject.transform.parent = transform;
                ultimate.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                ultimate.GetComponent<DragDrop>().canBePlaced = true;

                // Add Money
                BuyScreenManager.instance.AddMoney(25);

                //DESTROY THE VEHICLE
                Destroy(ultimate.gameObject);
            }
            else if (car != null)
            {
                car.gameObject.transform.parent = transform;
                car.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                car.GetComponent<DragDrop>().canBePlaced = true;

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
                Destroy(car.gameObject);
            }
        }
        
    }

    public void UpdateBGColour()
    {
        if (slotType == SlotType.CarShop)
        {
            BuyScreenCar car = transform.GetComponentInChildren<BuyScreenCar>();
            BuyScreenUltimate ultimate = transform.GetComponentInChildren<BuyScreenUltimate>();

            if (car != null)
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
            else if (ultimate != null)
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
