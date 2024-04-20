using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyScreenInfoBox : MonoBehaviour
{
    [SerializeField] private GameObject infoTitle;
    [SerializeField] private GameObject infoDescription;

    [HideInInspector] public static BuyScreenInfoBox instance;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }

        //Fetch the infoTitle and infoDescription from the parent object

        infoTitle = transform.GetChild(0).gameObject;
        infoDescription = transform.GetChild(1).gameObject;

    }

    public void FillInfoBoxCar(BuyScreenCar car)
    {
        infoTitle.GetComponent<TextMeshProUGUI>().text = car.correspondingCar.GetComponent<ObjectInfo>().objectName;
        infoDescription.GetComponent<TextMeshProUGUI>().text = car.correspondingCar.GetComponent<ObjectInfo>().objectDescription;
    }

    public void FillInfoBoxUltimate(BuyScreenUltimate ultimate)
    {
        infoTitle.GetComponent<TextMeshProUGUI>().text = ultimate.correspondingUltimate.GetComponent<ObjectInfo>().objectName;
        infoDescription.GetComponent<TextMeshProUGUI>().text = ultimate.correspondingUltimate.GetComponent<ObjectInfo>().objectDescription;
    }
}
