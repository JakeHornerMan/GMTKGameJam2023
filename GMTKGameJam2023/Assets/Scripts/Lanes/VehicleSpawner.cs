using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Car selectedCar;
    [SerializeField] private Transform spawnedVehiclesContainer;

    [Header("Input")]
    [SerializeField] private int placeMouseBtn = 0;

    [Header("Tags")]
    [SerializeField] private string roadTag = "Road";

    [Header("Spawn Positioning")]
    [SerializeField] private float spawnZoneDivider = 0;
    [SerializeField] private Vector2 spawnOffset = new(0, 5);

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(placeMouseBtn))
            PlaceVehicle();
    }

    private void PlaceVehicle()
    {
        // Raycast toward Click
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // Return if Clicked Nothing
        if (hit.collider == null)
            return;

        // Return if Did Not Click Road
        if (!hit.collider.gameObject.CompareTag(roadTag))
            return;

        Vector3 spawnPos = hit.collider.transform.position;
        Vector3 spawnRot = new();

        // Clicked Top Half - Spawn on Top
        if (hit.point.y > spawnZoneDivider)
        {
            spawnPos += (Vector3)spawnOffset;
            spawnRot = new Vector3(0, 0, 180);
        }

        // Clicked Bottom Half - Spawn on Bottom
        else if (hit.point.y <= spawnZoneDivider)
        {
            spawnPos -= (Vector3)spawnOffset;
            spawnRot = new Vector3(0, 0, 0);
        }

        // Spawn Car at Road at Position
        Instantiate(
            selectedCar.gameObject,
            spawnPos,
            Quaternion.Euler(spawnRot),
            spawnedVehiclesContainer
        );
        GameObject soundmanager = GameObject.Find("GameManger");
        soundmanager.GetComponent<SoundManager>().PlaySound("newCar");
    }
}
