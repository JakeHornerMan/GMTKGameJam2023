using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles spawning of buttons in a grid for all
/// vehicles, ultimates, and chicken.
/// </summary>
public class GuidebookSelector : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The grid-layout container to spawn blueprint icons into.")]
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private GameObject guideSelectButtonPrefab;

    [Header("Items to Display")]
    [SerializeField] private Car[] carsToShow;
    [SerializeField] private Ultimate[] ultimatesToShow;
    [SerializeField] private ObjectInfo[] chickenToShow;

    // Store current displayed type if needed
    public enum ViewType { Cars, Ultimates, Chicken }
    public ViewType currentViewType;

    private void Start()
    {
        // Show Cars by Default
        ShowCarButtons();
    }

    // Clear all buttons in grid layout container
    private void ClearGrid()
    {
        for (var i = buttonsContainer.transform.childCount - 1; i >= 0; i--)
            Destroy(buttonsContainer.transform.GetChild(i).gameObject);
    }

    // Run from UI by button
    public void ShowCarButtons()
    {
        currentViewType = ViewType.Cars;
        ClearGrid();
        foreach (Car car in carsToShow)
        {
            GuidebookButton btn = Instantiate(
                guideSelectButtonPrefab,
                buttonsContainer
            ).GetComponent<GuidebookButton>();
            btn.Creation(car);
        }
    }

    public void ShowChickenButtons()
    {
        currentViewType = ViewType.Chicken;
        ClearGrid();
        foreach (ObjectInfo chicken in chickenToShow)
        {
            GuidebookButton btn = Instantiate(
                guideSelectButtonPrefab,
                buttonsContainer
            ).GetComponent<GuidebookButton>();
            btn.Creation(chicken);
        }
    }

    public void ShowUltimateButtons()
    {
        currentViewType = ViewType.Ultimates;
        ClearGrid();
        foreach (Ultimate ultimate in ultimatesToShow)
        {
            GuidebookButton btn = Instantiate(
                guideSelectButtonPrefab,
                buttonsContainer
            ).GetComponent<GuidebookButton>();
            btn.Creation(ultimate);
        }
    }
}
