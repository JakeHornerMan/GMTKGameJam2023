using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHighlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bottomHighlight;
    [SerializeField] private GameObject shineObject;

    [Header("Settings")]
    [SerializeField] private bool allowShine = true;

    private VehicleSpawner vehicleSpawner;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
    }

    private void Start()
    {
        DisableShineObject();
    }

    private void Update()
    {
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (vehicleSpawner != null && vehicleSpawner.currentActiveCar != null)
        {
            bool amIPlaceable = vehicleSpawner.currentActiveCar.placeableLaneTags.Contains(gameObject.tag)
                                || vehicleSpawner.currentUltimateAbility.placeableAnywhere;
            if (hit.collider == null || !amIPlaceable)
            {
                bottomHighlight.SetActive(false);
                return;
            }
        }

        bool touchingRoad = hit.collider.gameObject == gameObject;

        bottomHighlight.SetActive(touchingRoad);
    }

    public void ShineLane()
    {
        shineObject.SetActive(true);
        Invoke(nameof(DisableShineObject), 0.8f);
    }

    public void DisableShineObject()
    {
        shineObject.SetActive(false);
    }
}
