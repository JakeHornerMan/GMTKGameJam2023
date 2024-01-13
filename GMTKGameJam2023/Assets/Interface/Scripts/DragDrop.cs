using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public Vector2 startingPosition;
    public Transform startingParent;

    public bool canBePlaced;

    public int itemPrice;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (GetComponent<CarButton>() != null)
        {
            itemPrice = GetComponent<CarButton>().correspondingCar.carPrice;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canBePlaced = false;
        Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        startingParent = transform.parent;
        transform.parent = canvas.transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;

        if (canBePlaced)
        {

        }
        else
        {
            transform.parent = startingParent;
            rectTransform.anchoredPosition = startingPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Goes off when an object (like a car) is dropped on top.
        //From this, IF the car has been dragged from the shop, the code needs to:
        //add pointerEnter's car to the scrapyard, add pointerDrag's car as the child of this item slot, take away any money that the car cost, and grey out the pointerDrag car in the shop


        if (transform.parent.GetComponent<BuyScreenCarSlot>() != null)
        {

            eventData.pointerEnter = transform.parent.gameObject;

            transform.parent.GetComponent<BuyScreenCarSlot>().OnDrop(eventData);
        }


        //if (eventData.pointerDrag.GetComponent<DragDrop>().startingParent.GetComponent<BuyScreenCarSlot>().slotType == BuyScreenCarSlot.SlotType.CarShop)
        //{
        //    //add pre-existing (pointerEnter) car to scrapyard

        //    BuyScreenManager.instance.AddToScrapyard(eventData.pointerEnter.transform.parent.GetComponent<CarButton>().correspondingCar);

            

        //    //add pointerDrag's car as the child of this item slot

        //    eventData.pointerDrag.gameObject.transform.parent = transform;
        //    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //    eventData.pointerDrag.GetComponent<DragDrop>().canBePlaced = true;

        //    //take away any money

        //    BuyScreenManager.instance.RemoveAmount(eventData.pointerDrag.GetComponent<CarButton>().correspondingCar.carPrice);
        //}
    }
}
