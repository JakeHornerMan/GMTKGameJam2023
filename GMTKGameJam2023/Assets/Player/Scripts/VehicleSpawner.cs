using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public CarButton standardCar;
    [SerializeField] public CarButton spikedCar;
    [SerializeField] public CarButton superCar;
    [SerializeField] public CarButton truck;
    [SerializeField] private Transform spawnedVehiclesContainer;
    [SerializeField] private CurrentCarIndicator carCursorFollower;

    [Header("Car Select Indicators")]
    [SerializeField] private Transform selectedCarIndicator;
    [SerializeField] private float standardCarX = 0;
    [SerializeField] private float spikedCarX = 0;
    [SerializeField] private float superCarX = 0;
    [SerializeField] private float truckX = 0;

    [Header("Input")]
    [SerializeField] private int placeMouseBtn = 0;

    [Header("Spawn Positioning")]
    [SerializeField] private Vector2 spawnOffset = new(0, -5);

    [Header("[Magnitude, DurationSeconds] of Camera Shake for Invalid Car Placement")]
    [SerializeField] private Vector2 invalidPlacementCamShake = new Vector2(0.15f, 0.2f);

    [HideInInspector] public Car currentActiveCar;

    private Camera mainCamera;
    private SoundManager soundManager;
    private GameManager gameManager;
    private CarWallet carWallet;
    private CameraShaker cameraShaker;

    private Vector3 mousePos;

    private void Awake()
    {
        mainCamera = Camera.main;
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        carWallet = GetComponent<CarWallet>();
    }

    private void Start()
    {
        SelectCar(standardCar);
    }

    private void Update()
    {
        if (gameManager.gameOver) return;

        if (Input.GetMouseButtonDown(placeMouseBtn))
            PlaceSelectedCar();

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
            SelectCar(standardCar);

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.W))
            SelectCar(superCar);

        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.E))
            SelectCar(spikedCar);

        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.R))
            SelectCar(truck);

        UpdateMousePos();
        UpdateCarCursor();
    }

    private void UpdateMousePos()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void PlaceSelectedCar()
    {
        // Check Money
        if (currentActiveCar.carPrice > gameManager.tokens)
            return;

        // Check Car Wallet Budget
        if (carWallet.carCount <= 0)
            return;

        // Raycast toward Click
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // Return if Clicked Nothing
        if (hit.collider == null)
            return;

        if (IsMouseOverUIElement())
            return;

        // Return if Car Cannot be Placed on Clicked Lane
        if (!currentActiveCar.placeableLaneTags.Contains(hit.collider.gameObject.tag))
        {
            StartCoroutine(cameraShaker.Shake(invalidPlacementCamShake.x, invalidPlacementCamShake.y));
            return;
        }

        // Spawn Car at Road at Position
        Vector3 spawnPos;
        if (currentActiveCar.placeableAnywhere)
            spawnPos = new Vector3(mousePos.x, mousePos.y, 1);
        else
            spawnPos = hit.collider.transform.position + (Vector3)spawnOffset;

        Instantiate(
            currentActiveCar.gameObject,
            spawnPos,
            Quaternion.identity,
            spawnedVehiclesContainer
        );

        // Reduce Car Wallet Count
        carWallet.carCount--;

        // Reduce Player Money
        gameManager.tokens -= currentActiveCar.carPrice;
        SelectCar(standardCar);
    }

    public void SelectCar(CarButton carBtn)
    {
        currentActiveCar = carBtn.correspondingCar;

        float x;
        if (carBtn == standardCar)
            x = standardCarX;
        else if (carBtn == superCar)
            x = superCarX;
        else if (carBtn == spikedCar)
            x = spikedCarX;
        else if (carBtn == truck)
            x = truckX;
        else
            x = standardCarX;

        selectedCarIndicator.transform.position = new Vector3(x, selectedCarIndicator.transform.position.y, 0);
    }

    private void UpdateCarCursor()
    {
        if (carCursorFollower.followCursor)
            carCursorFollower.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        carCursorFollower.SetUI(currentActiveCar, gameManager.tokens, carWallet.carCount);
    }

    private bool IsMouseOverUIElement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
        if (hit.collider != null && hit.collider.GetComponent<GraphicRaycaster>() != null)
            return true;
        return false;
    }
}
