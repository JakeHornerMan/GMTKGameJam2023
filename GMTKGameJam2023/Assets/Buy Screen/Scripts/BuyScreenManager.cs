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

    [SerializeField] public Ultimate playerUltimate;

    [Header("Car Appearance Order")]
    [SerializeField] private Car[] cars;
    [SerializeField] private Ultimate[] ultimates;

    [Header("Menu Prices")]
    public int startingAmount;
    private int currentAmount;

    [SerializeField] private int lifePrice;
    [SerializeField] private int walletPrice;
    [SerializeField] private int energyPrice;
    [SerializeField] private int rerollPrice;

    [SerializeField] private int maxRerolls; // Maximum number of rerolls allowed
    private int remainingRerolls; // Tracks remaining rerolls

    [Header("Other")]
    [SerializeField] private GameObject RosterHolder;

    [SerializeField] private GameObject carShop;
    [SerializeField] private GameObject ultimateShop;

    [SerializeField] private GameObject rerollButton;

    [SerializeField] private GameObject scrapyard;

    [SerializeField] private GameObject rosterCarPrefab;
    [SerializeField] private GameObject rosterUltimatePrefab;

    [SerializeField] private SceneFader sceneFader;

    public static BuyScreenManager instance;

    //Health Properties
    [SerializeField] private Slider healthSlider;
    private static float[] healthSliderValues = new float[] { 0, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.75f, 0.85f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI healthNumberText;

    //Wallet Properties
    [SerializeField] private Slider walletSlider;
    private static float[] walletSliderValues = new float[] { 0, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.75f, 0.85f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI walletNumberText;

    //Energy Properties
    [SerializeField] private Slider energySlider;
    private int energyIncrement;
    [SerializeField] private int energyMultiplier = 5;
    private static float[] energySliderValues = new float[] { 0, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.75f, 0.85f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI energyNumberText;

    [SerializeField] private TextMeshProUGUI moneyText;



    private void Awake()
    {
        SetPlayerValuesInBuyScreen();
        PopulateShops();
    }

    void Start()
    {
        // CreateRosterList();

        if (instance == null)
        {
            instance = this;
        }

        sceneFader.gameObject.SetActive(true);

        currentAmount = startingAmount + PlayerValues.playerCash;

        UpdateMoneyText();

        CalculateIncrements();

        UpdateHealthBar();
        UpdateWalletBar();
        UpdateEnergyBar();

        remainingRerolls = maxRerolls; // Initialize remaining rerolls
        UpdateRerollCounter(); // Update visual counter


    }

    private void SetPlayerValuesInBuyScreen(){
        if(!devMode){
            if(PlayerValues.Cars != null){
                playerCars = PlayerValues.Cars;
                // Debug.Log("Cars: " + playerCars[0]);
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

        for (int i = 0; i < playerCars.Count; i++)
        {
            Transform child = RosterHolder.transform.GetChild(i);
            
            GameObject newBuyCar = Instantiate(rosterCarPrefab, child.transform);

            newBuyCar.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            BuyScreenCar buyScreenCar = child.GetComponentInChildren<BuyScreenCar>();
            
            buyScreenCar.correspondingCar = playerCars[i];
            Debug.Log("We have set the carbutton to" + buyScreenCar.correspondingCar);
        }


    }

    public void UpdatePlayerCarsList()
    {
        if (playerCars != null)
        {
            playerCars.Clear();
        }

        playerCars = CreateRosterList();
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


    void PopulateShops()
    {
        PopulateCarShop();
        PopulateUltimateShop();
    }

    public void PopulateCarShop()
    {
        List<Car> carsPulled = new List<Car>();

        for (int i = 0; i < carShop.transform.childCount; i++)
        {

            Car car = cars[0];

            int randomNumber = 0;

            bool newCar = false;

            while (!newCar)
            {
                randomNumber = Random.Range(0, cars.Length);
                car = cars[randomNumber];

                bool isUnique = true;

                for (int j = 0; j < carsPulled.Count; j++)
                {
                    if (car == carsPulled[j])
                    {
                        isUnique = false;
                        break;
                    }
                }

                for (int j = 0; j < playerCars.Count; j++)
                {
                    if (car == playerCars[j])
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    newCar = true;
                    carsPulled.Add(car);
                }
            }


            if (carShop.transform.GetChild(i).transform.childCount == 0)
            {
                Instantiate(rosterCarPrefab, carShop.transform.GetChild(i).transform);
            }

            BuyScreenCar carSlot = carShop.transform.GetChild(i).GetChild(0).gameObject.GetComponent<BuyScreenCar>();

            carSlot.correspondingCar = car;

            carSlot.UpdateSprite();

            carSlot.gameObject.GetComponent<Animator>().Play("RerollShake");

        }
    }


    public void PopulateUltimateShop()
    {
        List<Ultimate> ultimatesPulled = new List<Ultimate>();

        for (int i = 0; i < ultimateShop.transform.childCount; i++)
        {

            Ultimate ultimate = ultimates[0];

            int randomNumber = 0;

            bool newUltimate = false;

            while (!newUltimate)
            {
                randomNumber = Random.Range(0, ultimates.Length);
                ultimate = ultimates[randomNumber];

                bool isUnique = true;

                //for (int j = 0; j < ultimatesPulled.Count; j++)
                //{
                //    if (ultimate == ultimatesPulled[j])
                //    {
                //        isUnique = false;
                //        break;
                //    }
                //}

                if (ultimate == playerUltimate)
                {
                    isUnique = false;
                    break;
                }

                if (isUnique)
                {
                    newUltimate = true;
                    ultimatesPulled.Add(ultimate);
                }
            }


            if (ultimateShop.transform.GetChild(i).transform.childCount == 0)
            {
                Instantiate(rosterCarPrefab, ultimateShop.transform.GetChild(i).transform);
            }

            BuyScreenUltimate ultimateSlot = ultimateShop.transform.GetChild(i).GetChild(0).gameObject.GetComponent<BuyScreenUltimate>();

            ultimateSlot.correspondingUltimate = ultimate;

            ultimateSlot.UpdateSprite();

            ultimateSlot.gameObject.GetComponent<Animator>().Play("RerollShake");

        }
    }

    public void RerollShop()
    {
        if (CheckMoneyAmount(rerollPrice) && remainingRerolls > 0)
        {
            RemoveMoney(rerollPrice);
            remainingRerolls--;
            UpdateRerollCounter(); // Update visual counter
            UpdatePlayerCarsList();
            PopulateCarShop();
        }
    }

    void UpdateRerollCounter()
    {
        if (remainingRerolls > 0)
        {
            rerollButton.GetComponent<RerollButton>().SetDiceFace(remainingRerolls);
        }
        else
        {
            rerollButton.GetComponent<RerollButton>().DisableDice();
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

    public void AddWallet(int value)
    {
        if (CheckMoneyAmount(walletPrice))
        {
            if (PlayerValues.carWalletNodes < 8)
            {
                PlayerValues.carWalletNodes = PlayerValues.carWalletNodes + value;

                RemoveMoney(walletPrice);
            }
        }

        UpdateWalletBar();
    }

    public void AddEnergy(int value)
    {
        if (CheckMoneyAmount(energyPrice))
        {
            if (PlayerValues.startingEnergy < 40)
            {
                energyIncrement = energyIncrement + value;

                PlayerValues.startingEnergy = energyIncrement * energyMultiplier;

                RemoveMoney(energyPrice);
            }
        }

        UpdateEnergyBar();
    }

    private void UpdateHealthBar()
    {
        healthNumberText.text = PlayerValues.missedChickenLives.ToString();

        healthSlider.value = healthSliderValues[PlayerValues.missedChickenLives];
    }

    private void UpdateWalletBar()
    {
        walletNumberText.text = PlayerValues.carWalletNodes.ToString();

        walletSlider.value = walletSliderValues[PlayerValues.carWalletNodes];
    }

    private void UpdateEnergyBar()
    {
        energyNumberText.text = PlayerValues.startingEnergy.ToString();

        energySlider.value = energySliderValues[energyIncrement];
    }

    private void CalculateIncrements()
    {
        energyIncrement = PlayerValues.startingEnergy / energyMultiplier;
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
        moneyText.text = "Cash: " + currentAmount.ToString();
    }

    public bool CheckMoneyAmount(int value)
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
        sceneFader.ScreenWipeOut("ProceduralGeneration");
    }

    public void setPlayerValues()
    {
        PlayerValues.Cars = CreateRosterList();
        PlayerValues.playerCash = 0;
    }
}
