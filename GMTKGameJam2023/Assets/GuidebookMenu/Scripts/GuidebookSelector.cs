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
    [SerializeField] private ChickenMovement[] chickensToShow;
    [SerializeField] private Ultimate[] ultimatesToShow;

    // Store current displayed type if needed
    private enum ViewType { Cars, Ultimates, Chicken }
    private ViewType currentViewType;

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
        foreach (ChickenMovement chicken in chickensToShow)
        {
            GuidebookButton btn = Instantiate(
                guideSelectButtonPrefab,
                buttonsContainer
            ).GetComponent<GuidebookButton>();
            btn.Creation(chicken);
        }
    }
}
