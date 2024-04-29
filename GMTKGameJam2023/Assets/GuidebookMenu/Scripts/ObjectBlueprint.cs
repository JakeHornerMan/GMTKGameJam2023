using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Runtime.CompilerServices;

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
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectDescriptionText;
    [SerializeField] private TextMeshProUGUI objectTypeText;

    [Tooltip("Only Vehicles: Cost of tokens to place")]
    [SerializeField] private TextMeshProUGUI tokenCostText;
    [Tooltip("Only Vehicles & Ultimate: Cost to put in inventory from shop")]
    [SerializeField] private TextMeshProUGUI shopUnlockCostText;
    [Tooltip("Only Vehicles: GridLayout container which contains the placeable roads.")]
    [SerializeField] private GridLayoutGroup laneLabelGridContainer;
    [Tooltip("Shows name of lane along with image.")]
    [SerializeField] private LaneLabel laneLabelPrefab;

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
        objectNameText.text = string.Empty;
        objectDescriptionText.text = string.Empty;
        objectImage.sprite = null;
        objectTypeText.text = string.Empty;

        // Hide token text and its parent display box
        tokenCostText.text = string.Empty;
        tokenCostText.transform.parent.gameObject.SetActive(false);

        // Hide shop text and its parent display box
        shopUnlockCostText.text = string.Empty;
        shopUnlockCostText.transform.parent.gameObject.SetActive(false);

        // Clear all road labels and hide road labels container
        for (var i = laneLabelGridContainer.transform.childCount - 1; i >= 0; i--)
            Destroy(laneLabelGridContainer.transform.GetChild(i).gameObject);
        laneLabelGridContainer.gameObject.SetActive(false);

        // Enabling makes animation run every time something new is displayed
        // i.e. When scrolling using left/right arrows
        // gameObject.SetActive(false);
    }

    /// <summary>
    /// General function for all three: cars, ultimates, and chicken.
    /// Just sets name, image, and description, which all 3 have in ObjectInfo.
    /// </summary>
    private void ShowObjectInfo(ObjectInfo obj)
    {
        gameObject.SetActive(true);
        objectNameText.text = obj.objectName;
        objectDescriptionText.text = obj.objectDescription;
        objectImage.sprite = obj.objectSprite;
        currentlyDisplayedObject = obj;
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

    // Handle Click on Close Button from UI
    public void HandleClose()
    {
        ClearUI();
        gameObject.SetActive(false);
    }
}
