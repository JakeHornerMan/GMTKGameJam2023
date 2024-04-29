using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [Header("Navigation Buttons")]
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject prevBtn;

    [Header("Items to Display")]
    [SerializeField] private List<Car> carsToShow;
    [SerializeField] private List<Ultimate> ultimatesToShow;
    [SerializeField] private List<ObjectInfo> chickenToShow;

    // Store current displayed type if needed
    public enum ViewType { Cars, Ultimates, Chicken }
    public ViewType currentViewType;

    private ObjectBlueprint objectBlueprint;

    private void Awake()
    {
        objectBlueprint = FindObjectOfType<ObjectBlueprint>();
    }

    private void Start()
    {
        // Show Cars by Default
        ShowCarButtons();
    }

    private void Update()
    {
        if (objectBlueprint.gameObject.activeInHierarchy)
        {
            nextBtn.SetActive(true);
            prevBtn.SetActive(true);
        }
        else
        {
            nextBtn.SetActive(false);
            prevBtn.SetActive(false);
        }
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

    // Handle Next and Previous Button
    public void ShowNextItem()
    {
        if (currentViewType == ViewType.Cars)
        {
            Car currentCar = objectBlueprint.currentlyDisplayedObject.GetComponent<Car>();
            int currentIndex = carsToShow.IndexOf(currentCar);
            int nextIndex = currentIndex + 1;

            // Next index does not exist, go back to first
            if (nextIndex == carsToShow.Count) nextIndex = 0;

            objectBlueprint.DisplayInfo(carsToShow[nextIndex]);
        }

        if (currentViewType == ViewType.Ultimates)
        {
            Ultimate currentUltimate = objectBlueprint.currentlyDisplayedObject.GetComponent<Ultimate>();
            int currentIndex = ultimatesToShow.IndexOf(currentUltimate);
            int nextIndex = currentIndex + 1;

            // Next index does not exist, go back to first
            if (nextIndex == ultimatesToShow.Count) nextIndex = 0;

            objectBlueprint.DisplayInfo(ultimatesToShow[nextIndex]);
        }

        if (currentViewType == ViewType.Chicken)
        {
            ObjectInfo currentChicken = objectBlueprint.currentlyDisplayedObject;
            int currentIndex = chickenToShow.IndexOf(currentChicken);
            int nextIndex = currentIndex + 1;

            // Next index does not exist, go back to first
            if (nextIndex == chickenToShow.Count) nextIndex = 0;

            objectBlueprint.DisplayInfo(chickenToShow[nextIndex]);
        }
    }

    public void ShowPrevItem()
    {
        if (currentViewType == ViewType.Cars)
        {
            Car currentCar = objectBlueprint.currentlyDisplayedObject.GetComponent<Car>();
            int currentIndex = carsToShow.IndexOf(currentCar);
            int prevIndex = currentIndex - 1;

            // Prev index does not exist, go back to first
            if (prevIndex < 0)
                objectBlueprint.DisplayInfo(carsToShow[^1]);
            // Prev Index is valid
            else
                objectBlueprint.DisplayInfo(carsToShow[prevIndex]);
        }

        if (currentViewType == ViewType.Ultimates)
        {
            Ultimate currentUltimate = objectBlueprint.currentlyDisplayedObject.GetComponent<Ultimate>();
            int currentIndex = ultimatesToShow.IndexOf(currentUltimate);
            int prevIndex = currentIndex - 1;

            // Prev index does not exist, go back to first
            if (prevIndex < 0)
                objectBlueprint.DisplayInfo(ultimatesToShow[^1]);
            // Prev Index is valid
            else
                objectBlueprint.DisplayInfo(ultimatesToShow[prevIndex]);
        }

        if (currentViewType == ViewType.Chicken)
        {
            ObjectInfo currentChicken = objectBlueprint.currentlyDisplayedObject;
            int currentIndex = chickenToShow.IndexOf(currentChicken);
            int prevIndex = currentIndex - 1;

            // Prev index does not exist, go back to first
            if (prevIndex < 0)
                objectBlueprint.DisplayInfo(chickenToShow[^1]);
            // Prev Index is valid
            else
                objectBlueprint.DisplayInfo(chickenToShow[prevIndex]);
        }
    }
}
