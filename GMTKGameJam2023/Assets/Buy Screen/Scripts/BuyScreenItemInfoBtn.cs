using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyScreenItemInfoBtn : MonoBehaviour
{
    [SerializeField] private BuyScreenCar correspCar;
    [SerializeField] private BuyScreenUltimate correspUlt;
    [SerializeField] private ObjectInfo upgradeBar;
    [SerializeField] private ObjectInfo otherObject;

    [Header("RosterSlot References")]
    [SerializeField] private bool IsRosterSlot = false;
    // [SerializeField] private ObjectBlueprint objectBlueprint;


    public void Start(){
        if(IsRosterSlot){
            DisableOrEnable(correspCar, correspUlt);
        }
    }

    public void DisableOrEnable(BuyScreenCar car, BuyScreenUltimate ult){
        if(car == null && ult == null){
            DisableInfo();
        }
        else{
            ActiveInfo(car, ult);
        }
    }

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
            Debug.LogError("Issue with BuyScreenItemInfoBtn");
            return;
    }

    public void UpdateInfo()
    {
        if (correspCar != null)
            correspCar = gameObject.transform.parent.GetComponentInChildren<BuyScreenCar>();
        else if (correspUlt != null)
            correspUlt = gameObject.transform.parent.GetComponentInChildren<BuyScreenUltimate>();
    }


    public void ActiveInfo(BuyScreenCar car, BuyScreenUltimate ult){
        // this.gameObject.SetActive(true);
        Debug.Log("InforButtonCall!");
        correspCar = car;
        correspUlt = ult;
        // gameObject.SetActive(false);
        //this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.399869949f,0.399869949f,0.999674976f);
        // this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
    }

    public void DisableInfo(){
        correspCar = null;
        correspUlt = null;
        // this.gameObject.SetActive(false);
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0f,0f,0f);
    }

    
}
