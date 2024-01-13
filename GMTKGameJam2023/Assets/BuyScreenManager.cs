using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyScreenManager : MonoBehaviour
{




    [SerializeField] private GameObject RosterHolder;

    [SerializeField] private GameObject scrapyard;

    [SerializeField] private GameObject rosterCarPrefab;

    public static BuyScreenManager instance;

    [SerializeField] private TextMeshProUGUI moneyText;
    public int startingAmount;
    private int currentAmount;

    // Start is called before the first frame update
    void Start()
    {
        CreateRosterList();

        if (instance == null)
        {
            instance = this;
        }

        currentAmount = startingAmount;

        UpdateMoneyText();

        
    }

    public List<Car> CreateRosterList()
    {
        // Get a list/array from all children of "RosterHolder" that have the CarButton component, and return it

        List<Car> rosterCars = new List<Car>();

        // Iterate over each child of the RosterHolder
        for (int i = 0; i < RosterHolder.transform.childCount; i++)
        {
            Transform child = RosterHolder.transform.GetChild(i);
            CarButton carButton = child.GetComponentInChildren<CarButton>();

            // First check if the CarButton component exists
            if (carButton != null)
            {
                // Then check if the correspondingCar is not null
                if (carButton.correspondingCar != null)
                {
                    rosterCars.Add(carButton.correspondingCar);
                }
            }
        }

        return rosterCars;

        // Now, rosterCars contains all CarButton components from the children of RosterHolder
    }

    public void AddToScrapyard(Car car)
    {
        if (car != null)
        {

            GameObject carSlot = FindScrapyardSlot();

            GameObject carRoster = rosterCarPrefab;
            carRoster.GetComponent<CarButton>().correspondingCar = car;

            if (carSlot != null)
            {
                Instantiate(carRoster, carSlot.transform);
            }
        }
    }

    public GameObject FindScrapyardSlot()
    {
        for (int i = 0; i < scrapyard.transform.childCount; i++)
        {
            if (scrapyard.transform.GetChild(i).GetComponent<BuyScreenCarSlot>() != null)
            {
                if (scrapyard.transform.GetChild(i).childCount == 0)
                {
                    return scrapyard.transform.GetChild(i).gameObject;
                }
            }
        }
        return null;
    }

    public void AddAmount(int value)
    {

        currentAmount = currentAmount + value;

        UpdateMoneyText();
    }

    public void RemoveAmount(int value)
    {
        if (currentAmount > 0)
        {
            currentAmount = currentAmount - value;
        }

        UpdateMoneyText();

    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money: " + currentAmount.ToString();
    }
}
