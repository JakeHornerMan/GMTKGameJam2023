using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyScreenItemInfoBtn : MonoBehaviour
{
    [SerializeField] private BuyScreenCar correspCar;
    [SerializeField] private BuyScreenUltimate correspUlt;
    [SerializeField] private ObjectInfo upgradeBar;
    [SerializeField] private ObjectInfo otherObject;

    public void ShowInfo()
    {
        if (correspCar != null)
            FindObjectOfType<ObjectBlueprint>(true).DisplayInfo(correspCar.correspondingCar);
        else if (upgradeBar != null)
            FindObjectOfType<ObjectBlueprint>(true).ShowObjectInfo(upgradeBar, "Upgrade");
        else if (correspUlt != null)
            FindObjectOfType<ObjectBlueprint>(true).DisplayInfo(correspUlt.correspondingUltimate);
        else if (otherObject != null)
        {
            FindObjectOfType<ObjectBlueprint>(true).ShowObjectInfo(otherObject, "Other");
        }
        else
            return;
    }
}
