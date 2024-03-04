using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyScreenManager : MonoBehaviour
{
    [Header("Developer Settings")]
    [SerializeField] public bool devMode = false;
    [SerializeField] public List<Car> playerCars;
    [SerializeField] public List<Car> defaultCars;

    [Header("Car Appearance Order")]
    [SerializeField] private Car[] cars;

    [Header("Menu Prices")]
    [SerializeField] private int lifePrice;
    [SerializeField] private int rerollPrice;


    [SerializeField] private GameObject RosterHolder;

    [SerializeField] private GameObject carShop;

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


    private void Awake()
    {
        PopulateCarShop();
    }

    // Start is called before the first frame update
    private void Awake()
    {
        SetPlayerValuesInBuyScreen();
    }

    void Start()
    {
        // CreateRosterList();

        if (instance == null)
        {
            instance = this;
        }

        sceneFader.gameObject.SetActive(true);

        currentAmount = startingAmount;

        UpdateMoneyText();

        UpdateHealthBar();


    }

    private void SetPlayerValuesInBuyScreen(){
        if(!devMode){
            if(PlayerValues.Cars != null){
                playerCars = PlayerValues.Cars;
                Debug.Log("Cars: " + playerCars[0]);
            }
            else{
                playerCars = defaultCars;
            }
            PopulateRoster();   
        }
        else{
            //we can add some devMode settings
        }
    }

    public void PopulateRoster()
    {
        // Iterate over each child of the RosterHolder
        for (int i = 0; i < RosterHolder.transform.childCount; i++)
        {
            Transform child = RosterHolder.transform.GetChild(i);
            CarButton carButton = child.GetComponentInChildren<CarButton>();

            // First check if the CarButton component exists
            if (carButton != null)
            {
                // set Corresponding Car
                if(i < playerCars.Count){
                    carButton.correspondingCar = playerCars[i];
                    Debug.Log("We have set the carbutton to" + carButton.correspondingCar);
                }
                else{
                    carButton.correspondingCar = null;
                }
                
            }
        }
    }

    public List<Car> CreateRosterList()
    {
        // Get a list/array from all children of "RosterHolder" that have the CarButton component, and return it

        List<Car> rosterCars = new List<Car>();

        // Iterate over each child of the RosterHolder
        for (int i = 0; i < RosterHolder.transform.childCount; i++)
        {
            Transform child = RosterHolder.transform.GetChild(i);
            BuyScreenCar buyScreenCar = child.GetComponentInChildren<BuyScreenCar>();

            // First check if the CarButton component exists
            if (buyScreenCar != null)
            {
                // Then check if the correspondingCar is not null
                if (buyScreenCar.correspondingCar != null)
                {
                    rosterCars.Add(buyScreenCar.correspondingCar);
                }
            }
        }

        return rosterCars;

        // Now, rosterCars contains all CarButton components from the children of RosterHolder
    }


    public void PopulateCarShop()
    {
        List<int> carsPulled = new List<int>();

        for (int i = 0; i < carShop.transform.childCount; i++)
        {

            int randomNumber = 0;

            bool newCar = false;

            while (!newCar)
            {
                randomNumber = Random.Range(0, cars.Length);
                bool isUnique = true;

                for (int j = 0; j < carsPulled.Count; j++)
                {
                    if (randomNumber == carsPulled[j])
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    newCar = true;
                    carsPulled.Add(randomNumber);
                }
            }


            if (carShop.transform.GetChild(i).transform.childCount == 0)
            {
                Instantiate(rosterCarPrefab, carShop.transform.GetChild(i).transform);
            }

            BuyScreenCar carSlot = carShop.transform.GetChild(i).GetChild(0).gameObject.GetComponent<BuyScreenCar>();

            Car car = cars[randomNumber];

            carSlot.correspondingCar = car;

            carSlot.UpdateSprite();

            carSlot.gameObject.GetComponent<Animator>().Play("RerollShake");

        }
    }

    public void RerollShop()
    {
        if (CheckMoneyAmount(rerollPrice))
        {
            RemoveMoney(rerollPrice);

            //Do a shake animation

            

            PopulateCarShop();
        }
        
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
            if (PlayerValues.missedChickenLives < 8)
            {
                PlayerValues.missedChickenLives = PlayerValues.missedChickenLives + value;

                RemoveMoney(lifePrice);
            }
        }

        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthNumber.text = PlayerValues.missedChickenLives.ToString();

        healthSlider.value = healthSliderValues[PlayerValues.missedChickenLives];
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
        setPlayerValues();
        //Load next scene
        sceneFader.ScreenWipeOut("Level01");
    }

    public void setPlayerValues()
    {
        PlayerValues.Cars = CreateRosterList();
    }
}
