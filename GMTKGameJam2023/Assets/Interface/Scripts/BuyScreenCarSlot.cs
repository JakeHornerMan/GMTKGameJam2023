using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyScreenCarSlot : MonoBehaviour, IDropHandler
{

    public enum SlotType
    {
        Roster,
        CarShop,
        Scrapyard
    }

    public SlotType slotType;

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {

            if (transform.childCount > 0)
            {
                if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == SlotType.CarShop) // If the car came from the shop
                {
                    if (BuyScreenManager.instance.CheckMoneyAmount(eventData.pointerDrag.GetComponent<BuyScreenCar>().correspondingCar.carShopPrice)) // If the player has enough money to buy the car
                    {
                        // Add pre-existing (pointerEnter) car to scrapyard
                        GameObject oldCar = eventData.pointerEnter.GetComponentInChildren<DragDrop>().gameObject;
                        BuyScreenManager.instance.AddToScrapyard(oldCar);

                        // Add pointerDrag's car as the child of this item slot
                        eventData.pointerDrag.gameObject.transform.parent = transform;
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                        // Take away any money
                        BuyScreenManager.instance.RemoveMoney(eventData.pointerDrag.GetComponent<BuyScreenCar>().correspondingCar.carShopPrice);
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
                    if (BuyScreenManager.instance.CheckMoneyAmount(eventData.pointerDrag.GetComponent<BuyScreenCar>().correspondingCar.carShopPrice)) //if the player has enough money to buy the car)
                    {

                        eventData.pointerDrag.gameObject.transform.parent = transform;
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

                        //take away any money
                        BuyScreenManager.instance.RemoveMoney(eventData.pointerDrag.GetComponent<BuyScreenCar>().correspondingCar.carShopPrice);
                    }
                    else
                    {
                        eventData.pointerDrag.gameObject.transform.parent = eventData.pointerDrag.GetComponent<DragDrop>().startingParent;
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;
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
}
