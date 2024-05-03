using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows icon of attached object.
/// When clicked, brings up detailed blueprint view of that object.
/// </summary>
public class GuidebookButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image btnIcon;

    public enum ObjectType { Cars, Ultimates, Chicken }
    private ObjectType objectType;

    private Car assignedCar = null;
    private ObjectInfo assignedChicken = null;
    private Ultimate assignedUltimate = null;

    // Creation run by GuidebookSelector.cs when this button is instantiated.
    // Stores object along with type, and sets icon

    // Variation for Car
    public void Creation(Car assignedCar)
    {
        objectType = ObjectType.Cars;
        this.assignedCar = assignedCar;
        btnIcon.sprite = assignedCar.GetComponent<ObjectInfo>().objectIcon;
    }

    // Override for Ultimate
    public void Creation(Ultimate assignedUltimate)
    {
        objectType = ObjectType.Ultimates;
        this.assignedUltimate = assignedUltimate;
        btnIcon.sprite = assignedUltimate.GetComponent<ObjectInfo>().objectIcon;
    }

    // Override for Chicken
    public void Creation(ObjectInfo assignedChicken)
    {
        objectType = ObjectType.Chicken;
        this.assignedChicken = assignedChicken;
        btnIcon.sprite = assignedChicken.objectIcon;
    }

    // Function when button is clicked
    public void HandleClick()
    {
        ObjectBlueprint blueprint = FindObjectOfType<ObjectBlueprint>(true);
        if (objectType == ObjectType.Cars)
            blueprint.DisplayInfo(assignedCar);
        if (objectType == ObjectType.Ultimates)
            blueprint.DisplayInfo(assignedUltimate);
        if (objectType == ObjectType.Chicken)
            blueprint.DisplayInfo(assignedChicken);
    }
}
