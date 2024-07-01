using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;

/// <summary>
/// General purpose blueprint-style display, used on prefab.
/// Player can refer to this when they need to know about something in the game.
/// Object assigned through respective functions.
/// <br/><br/>
/// For Vehicles, Ultimates, and Chicken it shows: <br/>
/// - Name <br/>
/// - Description <br/>
/// - Image <br/>
/// <br/><br/>
/// For Vehicles and Ultimates, it also shows: <br/>
/// - Token cost <br/>
/// - Shop unlock cost <br/>
/// - List of Placeable lanes (for vehicles only, spawns labels for each type) <br/>
/// <br/><br/>
/// Car store prices and energy cost should only be defined in car script.
/// Ultimate store prices should only be defined in ultimate script.
/// </summary>

public class ObjectBlueprint : MonoBehaviour
{
    [Header("Tutorial Setting")]
    [SerializeField] public bool isTutorial = false;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectDescriptionText;
    [SerializeField] private TextMeshProUGUI objectTypeText;
    [SerializeField] private GameObject nextRoundContainer;
    [SerializeField] private TextMeshProUGUI nextRoundText;

    [SerializeField] private GameObject closeBtn;
    [SerializeField] private GameObject continueBtn;

    [Tooltip("Only Vehicles: Cost of tokens to place")]
    [SerializeField] private TextMeshProUGUI tokenCostText;
    [Tooltip("Only Vehicles & Ultimate: Cost to put in inventory from shop")]
    [SerializeField] private TextMeshProUGUI shopUnlockCostText;
    [Tooltip("Only Vehicles: GridLayout container which contains the placeable roads.")]
    [SerializeField] private GridLayoutGroup laneLabelGridContainer;
    [Tooltip("Shows name of lane along with image.")]
    [SerializeField] private LaneLabel laneLabelPrefab;
    [Tooltip("For lanes Blue Print in BuyScreen")]
    [SerializeField] private Image[] laneImages;
    [SerializeField] private GameObject lanesContainer;

    [Header("Testing Values")]
    [SerializeField] private Car debugCar;
    [SerializeField] private Ultimate debugUltimate;
    [SerializeField] private ObjectInfo debugChicken;

    public ObjectInfo currentlyDisplayedObject;

    private void Start()
    {
        ClearUI();

        if (debugCar != null)
            DisplayInfo(debugCar);
        else if (debugChicken != null)
            DisplayInfo(debugChicken);
        else if (debugUltimate != null)
            DisplayInfo(debugUltimate);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Clears the UI Before displaying the object info, 
    /// Hides information fields not relevant to type of displayed object.
    /// </summary>
    public void ClearUI()
    {
        // Hide Name, Description, Image
        if (objectNameText != null) objectNameText.text = string.Empty;
        if (objectDescriptionText != null) objectDescriptionText.text = string.Empty;
        if (objectImage != null) objectImage.sprite = null;
        if (objectTypeText != null) objectTypeText.text = string.Empty;

        // Hide token text and its parent display box
        if (tokenCostText != null) tokenCostText.text = string.Empty;
        if (tokenCostText != null) tokenCostText.transform.parent.gameObject.SetActive(false);

        // Hide shop text and its parent display box
        if (shopUnlockCostText != null) shopUnlockCostText.text = string.Empty;
        if (shopUnlockCostText != null) shopUnlockCostText.transform.parent.gameObject.SetActive(false);

        // Clear all road labels and hide road labels container
        if (laneLabelGridContainer != null)
        {
            for (var i = laneLabelGridContainer.transform.childCount - 1; i >= 0; i--)
                Destroy(laneLabelGridContainer.transform.GetChild(i).gameObject);
            laneLabelGridContainer.gameObject.SetActive(false);
        }

        // Enabling makes animation run every time something new is displayed
        // i.e. When scrolling using left/right arrows
        // gameObject.SetActive(false);

        // for when lanes need to be shown on buy screen i need to disable the object image
        if (objectImage != null) objectImage.enabled = true;
        if (lanesContainer != null) lanesContainer.SetActive(false);

        nextRoundContainer.SetActive(false);
        nextRoundText.gameObject.SetActive(false);
    }

    /// <summary>
    /// General function for all three: cars, ultimates, and chicken.
    /// Just sets name, image, and description, which all 3 have in ObjectInfo.
    /// </summary>
    public void ShowObjectInfo(ObjectInfo obj, string customType = "")
    {
        gameObject.SetActive(true);
        objectNameText.text = obj.objectName;
        objectDescriptionText.text = obj.objectDescription;
        objectImage.sprite = obj.bluePrintSprite ? obj.bluePrintSprite : obj.objectSprite;
        currentlyDisplayedObject = obj;
        if (customType != "")
        {
            objectTypeText.text = customType;
        }
    }

    /// <summary>
    /// Displays info for all types of cars. Includes basic ObjectInfo but also
    /// token cost and shop unlock price and placeable lane labels.
    /// </summary>
    /// <param name="vehicle">Vehicle for which to display the information.</param>
    /// <returns>Sends back the vehicle passed in.</returns>
    public Car DisplayInfo(Car vehicle)
    {
        ClearUI();
        objectTypeText.text = "Vehicle";
        gameObject.SetActive(true);

        if (vehicle.GetComponent<ObjectInfo>() != null)
            ShowObjectInfo(vehicle.GetComponent<ObjectInfo>());

        // Enable and set token cost view
        tokenCostText.text = vehicle.carPrice.ToString("00");
        tokenCostText.transform.parent.gameObject.SetActive(true);

        // Enable and set shop unlock cost view
        shopUnlockCostText.text = vehicle.carShopPrice.ToString("00");
        shopUnlockCostText.transform.parent.gameObject.SetActive(true);

        // Add labels for placeable lanes
        laneLabelGridContainer.gameObject.SetActive(true);
        foreach (string placeableLaneTag in vehicle.placeableLaneTags)
        {
            // Instantiate new labels
            LaneLabel newLabel = Instantiate(
                laneLabelPrefab.gameObject,
                laneLabelGridContainer.transform
            ).GetComponent<LaneLabel>();
            newLabel.SetLabelLane(placeableLaneTag);
        }

        return vehicle;
    }

    /// <summary>
    /// Override of Display Info for chicken information, 
    /// only shows objectInfo currently.
    /// </summary>
    /// <param name="chickenMovement">The Chicken whose information to display.</param>
    /// <returns>Sends back chicken object passed in.</returns>
    public ObjectInfo DisplayInfo(ObjectInfo chickenMovement)
    {
        ClearUI();
        objectTypeText.text = "Chicken";
        gameObject.SetActive(true);

        ShowObjectInfo(chickenMovement);

        return chickenMovement;
    }

    /// <summary>
    /// Override of Display Info for ultimate ability information, 
    /// Shows objectInfo and shop unlock price currently.
    /// </summary>
    /// <param name="ultimate">The Ultimate ability whose information to display.</param>
    /// <returns>Sends back ultimate object passed in.</returns>
    public Ultimate DisplayInfo(Ultimate ultimate)
    {
        ClearUI();
        objectTypeText.text = "Ultimate";
        gameObject.SetActive(true);

        ShowObjectInfo(ultimate.GetComponent<ObjectInfo>());

        // Enable and set shop unlock cost view
        shopUnlockCostText.text = ultimate.ultimateShopPrice.ToString("00");
        shopUnlockCostText.transform.parent.gameObject.SetActive(true);

        return ultimate;
    }

    /// <summary>
    /// Override of Display Info for Lanes in next level, 
    /// Shows all lane in order of appearance.
    /// </summary>
    /// <param name="laneMap">The list of lanes whose information to display.</param>
    /// <returns>Sends back null as object is only being sent to static script for next level to access</returns>
    public ObjectInfo DisplayInfo(List<GameObject> laneMap)
    {
        ClearUI();
        objectTypeText.text = "Lane Map";
        nextRoundContainer.SetActive(true);
        int nextRoundNumber = GameProgressionValues.RoundNumber;
        nextRoundText.text = $"Round {nextRoundNumber}";
        nextRoundText.gameObject.SetActive(true);
        gameObject.SetActive(true);

        if (lanesContainer != null) lanesContainer.SetActive(true);
        if (objectImage != null) objectImage.enabled = false;

        for (int i = 0; i < laneMap.Count; i++)
        {
            laneImages[i].sprite = laneMap[i].GetComponent<SpriteRenderer>().sprite;
        }

        return null;
    }

    // Handle Click on Close Button from UI
    public void HandleClose()
    {
        ClearUI();
        gameObject.SetActive(false);
    }

    public void DisplayDescription(string description)
    {
        gameObject.SetActive(true);
        objectDescriptionText.text = description;
    }

    public void activateContinue(bool isContinue = false)
    {
        if (isContinue)
        {
            closeBtn.SetActive(false);
            continueBtn.SetActive(true);
        }
        else
        {
            closeBtn.SetActive(true);
            continueBtn.SetActive(false);
        }
    }
}
