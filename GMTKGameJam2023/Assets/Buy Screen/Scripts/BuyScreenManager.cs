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

    [Header("Max Values")]
    [SerializeField] private float maxHealthAllowed;
    [SerializeField] private float maxWalletAllowed;
    [SerializeField] private float maxEnergyAllowed;

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
    public static Canvas canvasInstance;

    //Health Properties
    [Header("Health Properties")]
    [SerializeField] private Slider healthSlider;
    //private static float[] healthSliderValues = new float[] { 0, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.75f, 0.85f, 1 }; //its shit code, dont worry about it
    private static float[] healthSliderValues = new float[] { 0, 0.15f, 0.25f, 0.35f, 0.42f, 0.52f, 0.6f, 0.7f, 0.78f, 0.87f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI healthNumberText;
    [SerializeField] private Image healthPlusImage;

    //Wallet Properties
    [Header("Wallet Properties")]
    [SerializeField] private Slider walletSlider;
    private static float[] walletSliderValues = new float[] { 0, 0.15f, 0.25f, 0.35f, 0.42f, 0.52f, 0.6f, 0.7f, 0.78f, 0.87f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI walletNumberText;
    [SerializeField] private Image walletPlusImage;

    //Energy Properties
    [Header("Energy Properties")]
    [SerializeField] private Slider energySlider;
    private int energyIncrement;
    [SerializeField] private int energyMultiplier = 3;
    private static float[] energySliderValues = new float[] { 0, 0.15f, 0.25f, 0.35f, 0.42f, 0.52f, 0.6f, 0.7f, 0.78f, 0.87f, 1 }; //its shit code, dont worry about it
    [SerializeField] private TextMeshProUGUI energyNumberText;
    [SerializeField] private Image energyPlusImage;

    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Other References")]
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject sellPopup;

    public bool itemPurchased = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (canvasInstance == null)
        {
            canvasInstance = GetComponent<Canvas>();
        }



        SetPlayerValuesInBuyScreen();
        PopulateShops();

    }

    void Start()
    {
        // CreateRosterList();

        currentAmount = startingAmount + PlayerValues.playerCash;

        sceneFader.gameObject.SetActive(true);

        UpdateMoneyText();

        CalculateIncrements();

        UpdateHealthBar();
        UpdateWalletBar();
        UpdateEnergyBar();

        remainingRerolls = maxRerolls; // Initialize remaining rerolls
        UpdateRerollCounter(); // Update visual counter

        UpdateSlotColors();

        pointer.SetActive(false);
        itemPurchased = false;

        SellPopupClose();

        StartCoroutine(nameof(PlayPointer));
    }

    private void SetPlayerValuesInBuyScreen()
    {
        if (!devMode)
        {
            if (PlayerValues.Cars != null)
            {
                playerCars = PlayerValues.Cars;
                // Debug.Log("Cars: " + playerCars[0]);
            }
            else
            {
                playerCars = defaultCars;
            }
            if (PlayerValues.ultimate != null)
            {
                playerUltimate = PlayerValues.ultimate;
            }
            else
            {
                playerUltimate = null;
            }
            PopulateRoster();
        }
        else
        {
            //we can add some devMode settings
        }
    }

    public void PopulateRoster()
    {
        int count = (playerCars.Count > 5) ? 5 : playerCars.Count;
        for (int i = 0; i < count; i++)
        {
            Transform child = RosterHolder.transform.GetChild(i);

            GameObject newBuyCar = Instantiate(rosterCarPrefab, child.transform);
            newBuyCar.transform.SetSiblingIndex(1);

            newBuyCar.transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
            BuyScreenCar buyScreenCar = child.GetComponentInChildren<BuyScreenCar>();

            buyScreenCar.correspondingCar = playerCars[i];

            newBuyCar.GetComponent<DragDrop>().startingParent = child;

            child.gameObject.GetComponent<BuyScreenCarSlot>().correspondingInfoBtn.ActiveInfo(buyScreenCar, null);

            Debug.Log("We have set the carbutton to" + buyScreenCar.correspondingCar);
        }

        if (playerUltimate != null)
        {
            Transform child = RosterHolder.transform.GetChild(5);

            GameObject newBuyUltimate = Instantiate(rosterUltimatePrefab, child.transform);
            newBuyUltimate.transform.SetSiblingIndex(1);

            newBuyUltimate.transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
            BuyScreenUltimate buyScreenUltimate = child.GetComponentInChildren<BuyScreenUltimate>();

            buyScreenUltimate.correspondingUltimate = playerUltimate;

            newBuyUltimate.GetComponent<DragDrop>().startingParent = child;

            child.gameObject.GetComponent<BuyScreenCarSlot>().correspondingInfoBtn.ActiveInfo(null, buyScreenUltimate);

            Debug.Log("We have set the ultimateBtn to" + buyScreenUltimate.correspondingUltimate);
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

    public void UpdatePlayerUltimate()
    {
        playerUltimate = GetUltimateSetInBuyScreen();
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

    public Ultimate GetUltimateSetInBuyScreen()
    {
        Ultimate ultimate = null;
        Transform child = RosterHolder.transform.GetChild(5);
        BuyScreenUltimate buyScreenUltimate = child.GetComponentInChildren<BuyScreenUltimate>();
        if (buyScreenUltimate != null)
        {
            // Then check if the correspondingCar is not null
            if (buyScreenUltimate.correspondingUltimate != null)
            {
                ultimate = buyScreenUltimate.correspondingUltimate;
            }
        }
        return ultimate;
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


            if (carShop.transform.GetChild(i).GetComponentInChildren<BuyScreenCar>() == null)
            {
                Instantiate(rosterCarPrefab, carShop.transform.GetChild(i).transform);
            }

            //if (carShop.transform.GetChild(i).transform.childCount == 0)
            //{
            //    Instantiate(rosterCarPrefab, carShop.transform.GetChild(i).transform);
            //}

            BuyScreenCarSlot carSlot = carShop.transform.GetChild(i).gameObject.GetComponent<BuyScreenCarSlot>();

            BuyScreenCar buyScreenCar = carSlot.transform.GetComponentInChildren<BuyScreenCar>();

            buyScreenCar.correspondingCar = car;

            buyScreenCar.UpdateSprite();

            carSlot.UpdatePriceText();

            carSlot.correspondingInfoBtn.ActiveInfo(buyScreenCar, null);

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

                for (int j = 0; j < ultimatesPulled.Count; j++)
                {
                    if (ultimate == ultimatesPulled[j])
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (ultimate == playerUltimate)
                {
                    isUnique = false;
                }

                if (isUnique)
                {
                    newUltimate = true;
                    ultimatesPulled.Add(ultimate);
                }
            }

            if (ultimateShop.transform.GetChild(i).GetComponentInChildren<BuyScreenUltimate>() == null)
            {
                Instantiate(rosterUltimatePrefab, ultimateShop.transform.GetChild(i).transform);
            }

            BuyScreenCarSlot ultimateSlot = ultimateShop.transform.GetChild(i).gameObject.GetComponent<BuyScreenCarSlot>();

            BuyScreenUltimate buyScreenUltimate = ultimateSlot.transform.GetComponentInChildren<BuyScreenUltimate>();

            buyScreenUltimate.correspondingUltimate = ultimate;

            buyScreenUltimate.UpdateSprite();

            ultimateSlot.GetComponent<BuyScreenCarSlot>().UpdatePriceText();

            ultimateSlot.correspondingInfoBtn.ActiveInfo(null, buyScreenUltimate);

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
            UpdatePlayerUltimate();
            PopulateShops();
            UpdateSlotColors();
            
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


    void UpdateSlotColors()
    {
        for (int i = 0; i < carShop.transform.childCount; i++)
        {
            GameObject carSlot = carShop.transform.GetChild(i).gameObject;

            carSlot.GetComponent<BuyScreenCarSlot>().UpdateBGColour();
        }

        for (int i = 0; i < ultimateShop.transform.childCount; i++)
        {
            GameObject ultimateSlot = ultimateShop.transform.GetChild(i).gameObject;

            ultimateSlot.GetComponent<BuyScreenCarSlot>().UpdateBGColour();
        }

        UpdateUpgradeButtonColors();
    }

    void UpdateUpgradeButtonColors()
    {
        if (!CheckMoneyAmount(lifePrice))
        {
            healthPlusImage.enabled = false;
        }

        if (!CheckMoneyAmount(walletPrice))
        {
            walletPlusImage.enabled = false;
        }

        if (!CheckMoneyAmount(energyPrice))
        {
            energyPlusImage.enabled = false;
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
        if (PlayerValues.missedChickenLives >= 5)
        {
            return;
        }
        if (CheckMoneyAmount(lifePrice))
        {
            if (PlayerValues.missedChickenLives < maxHealthAllowed)
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
            if (PlayerValues.carWalletNodes < maxWalletAllowed)
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
            if (PlayerValues.startingEnergy < maxEnergyAllowed)
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

        GetComponent<SoundManager>().PlayPurchase();
    }

    public void RemoveMoney(int value)
    {
        if (currentAmount > 0)
        {
            currentAmount = currentAmount - value;
        }

        GetComponent<SoundManager>().PlayPurchase();

        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Cash: $" + currentAmount.ToString();

        UpdateSlotColors();
    }

    public bool CheckMoneyAmount(int value)
    {
        if (value <= currentAmount)
        {
            return true;
        }

        // GetComponent<SoundManager>().PlayCantPurchase();
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
        PlayerValues.ultimate = GetUltimateSetInBuyScreen();
        PlayerValues.playerCash = 0;

    }

    public IEnumerator PlayPointer()
    {
        yield return new WaitForSeconds(7f);
        if (!itemPurchased)
        {
            pointer.SetActive(true);
            StartCoroutine(nameof(WaitToHidePointer));
        }
    }

    private IEnumerator WaitToHidePointer()
    {
        // Wait until itemPurchased is true
        yield return new WaitUntil(() => itemPurchased);

        // Once itemPurchased is true, destroy the pointer
        Destroy(pointer);
    }

    public void SellPopup()
    {
        sellPopup.SetActive(true);
    }

    public void SellPopupClose()
    {
        sellPopup.SetActive(false);
    }
}
