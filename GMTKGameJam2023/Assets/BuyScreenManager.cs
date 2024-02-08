using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyScreenManager : MonoBehaviour
{


    [Header("Upgrade Prices")]
    [SerializeField] private int lifePrice;


    [SerializeField] private GameObject RosterHolder;

    [SerializeField] private GameObject scrapyard;

    [SerializeField] private GameObject rosterCarPrefab;

    [SerializeField] private SceneFader sceneFader;

    public static BuyScreenManager instance;

    //Health Properties
    [SerializeField] private Slider healthSlider;
    private static float[] healthSliderValues = new float[] { 0, 0.15f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.85f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI healthNumber;


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

        sceneFader.gameObject.SetActive(true);

        currentAmount = startingAmount;

        UpdateMoneyText();

        UpdateHealthBar();


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

    public void AddToScrapyard(GameObject car)
    {
        if (car != null)
        {

            GameObject carSlot = FindScrapyardSlot();

            if (carSlot != null)
            {
                car.transform.parent = carSlot.transform;
                car.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                car.GetComponent<DragDrop>().canBePlaced = true;


                //GameObject carRoster = Instantiate(rosterCarPrefab, carSlot.transform);
                //carRoster.GetComponent<CarButton>().correspondingCar = car.GetComponent<CarButton>().correspondingCar;
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


    public void AddLives(int value)
    {
        if (CheckMoneyAmount(lifePrice))
        {
            if (GameProgressionValues.chickenLives < 8)
            {
                GameProgressionValues.chickenLives = GameProgressionValues.chickenLives + value;

                RemoveMoney(lifePrice);
            }
        }

        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthNumber.text = GameProgressionValues.chickenLives.ToString();

        healthSlider.value = healthSliderValues[GameProgressionValues.chickenLives];
    }

    public void AddMoney(int value)
    {

        currentAmount = currentAmount + value;

        UpdateMoneyText();
    }

    public void RemoveMoney(int value)
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

    private bool CheckMoneyAmount(int value)
    {
        if (value <= currentAmount)
        {
            return true;
        }

        return false;
    }

    public void ToNextLevel()
    {
        CarStorage.Cars = CreateRosterList();

        sceneFader.ScreenWipeOut();

        //Load next scene
    }
}
