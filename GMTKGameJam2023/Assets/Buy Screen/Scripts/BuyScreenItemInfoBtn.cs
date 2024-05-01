using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyScreenItemInfoBtn : MonoBehaviour
{
    [SerializeField] private BuyScreenCar correspCar;
    [SerializeField] private BuyScreenUltimate correspUlt;

    public void ShowInfo()
    {
        if (correspCar != null)
            FindObjectOfType<ObjectBlueprint>(true).DisplayInfo(correspCar.correspondingCar);
        else
            FindObjectOfType<ObjectBlueprint>(true).DisplayInfo(correspUlt.correspondingUltimate);
    }
}
