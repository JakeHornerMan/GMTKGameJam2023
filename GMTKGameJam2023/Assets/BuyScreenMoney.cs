using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyScreenMoney : MonoBehaviour
{

    public int startingAmount;
    private int currentAmount;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Money: " + startingAmount.ToString();

        currentAmount = startingAmount;
    }


    void AddAmount(int value)
    {

        currentAmount = currentAmount + value;
    }

    void RemoveAmount(int value)
    {
        if (currentAmount > 0)
        {
            currentAmount = currentAmount - value;
        }
        
    }


}
