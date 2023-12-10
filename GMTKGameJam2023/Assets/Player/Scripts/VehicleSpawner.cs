using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Spawner Type")]
    [SerializeField] private bool isForWorldSelect = false;

    [Header("Placement Settings")]
    [SerializeField] private bool selectDefaultOnPlace = true;

    [Header("References")]
    [SerializeField] private Transform carSelectContainer;
    [SerializeField] private Transform spawnedVehiclesContainer;
    [SerializeField] private GameObject carButtonPrefab;
    [SerializeField] private Car standardCar;

    [Header("Input")]
    [SerializeField] private int placeMouseBtn = 0;

    [Header("Spawn Positioning")]
    [SerializeField] private Vector2 spawnOffset = new(0, -5);

    [Header("Spawn Settings")]
    [SerializeField] private float timeUntilNextSpawn;
    [SerializeField] public bool disableVehicleSpawn = false;

    [Header("[Magnitude, DurationSeconds] of Camera Shake for Invalid Car Placement")]
    [SerializeField] private Vector2 invalidPlacementCamShake = new(0.15f, 0.2f);

    [HideInInspector] public Car currentActiveCar;
    [HideInInspector] public Car ultimateAbility;

    private float currentTimeUntilNextSpawn;

    private Collider2D lastLaneSpawned;
    private CurrentCarIndicator currentCarIndicator;
    private Camera mainCamera;
    private GameManager gameManager;
    private Ultimate ultimate;
    private SoundManager soundManager;
    private CarWallet carWallet;
    private CameraShaker cameraShaker;

    private Vector3 inputPos;

    [HideInInspector] public List<CarButton> carButtons;

    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        ultimate = FindObjectOfType<Ultimate>();
        soundManager = FindObjectOfType<SoundManager>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        carWallet = GetComponent<CarWallet>();
        currentCarIndicator = FindObjectOfType<CurrentCarIndicator>();
    }

    private void Start()
    {
        CreateButtons();
    }

    private void Update()
    {
        if (gameManager.isGameOver) return;
        if (disableVehicleSpawn) return;

        if (SystemInfo.deviceType == DeviceType.Desktop)
            MouseInputs();

        if (SystemInfo.deviceType == DeviceType.Handheld)
            TouchInputs();

        //Timer that decreases to prevent players from spamming cars down a single lane (but still allow multiple lane spawning)
        if (currentTimeUntilNextSpawn > 0)
        {
            DecreaseSpawnTimer();
        }
    }

    private void CreateButtons()
    {
        foreach (Car car in gameManager.carsInLevel)
        {
            CarButton btn = Instantiate(
                carButtonPrefab,
                carSelectContainer
            ).GetComponent<CarButton>();
            carButtons.Add(btn);
            btn.correspondingCar = car;
        }

        if (carButtons.Count >= 1)
            currentActiveCar = standardCar;
    }

    private void MouseInputs()
    {
        if (Input.GetMouseButtonDown(placeMouseBtn))
            PlaceSelectedCar();

        if(Input.GetKeyDown(KeyCode.Space) && standardCar != null)
            SelectCar(standardCar);

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q) && carButtons.Count >= 1)
            SelectCar(carButtons[0].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.W) && carButtons.Count >= 2)
            SelectCar(carButtons[1].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.E) && carButtons.Count >= 3)
            SelectCar(carButtons[2].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.R) && carButtons.Count >= 4)
            SelectCar(carButtons[3].correspondingCar);

        UpdateMousePos();
        UpdateCarCursor();
    }

    private void TouchInputs()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    inputPos = mainCamera.ScreenToWorldPoint(touch.position);
                    PlaceSelectedCar();
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void UpdateMousePos()
    {
        inputPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void setStandardCar(){
        SelectCar(standardCar);
    }

    public void SetUltimate()
    {
        if(ultimate.isReady){
            SelectCar(ultimate.correspondingUltimate);
        }
    }

    private void PlaceSelectedCar()
    {
        // Check Money, Check Car Wallet Budget
        if (NotEnoughCarWallet())
            return;

        // Raycast toward Click
        RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero);

        // Return if Clicked Nothing
        if (hit.collider == null)
            return;

        if (IsMouseOverUIElement())
            return;

        // Return if Car Cannot be Placed on Clicked Lane
        if (isForWorldSelect)
        {
            if (hit.collider.GetComponent<WorldSelectLaneCar>() == null) return;
            currentActiveCar = hit.collider.GetComponent<WorldSelectLaneCar>().worldSelectCar;
        }
        else if (!currentActiveCar.placeableLaneTags.Contains(hit.collider.gameObject.tag))
        {
            StartCoroutine(cameraShaker.Shake(invalidPlacementCamShake.x, invalidPlacementCamShake.y));
            return;
        }

        // Spawn Car at Road at Position
        Vector3 spawnPos;
        if (currentActiveCar.placeableAnywhere)
            spawnPos = new Vector3(inputPos.x, inputPos.y, 1);
        else
            spawnPos = hit.collider.transform.position + (Vector3)spawnOffset;

        // To prevent car spamming on the same lane
        if (hit.collider == lastLaneSpawned && currentTimeUntilNextSpawn > 0)
            return;

        Instantiate(
            currentActiveCar.gameObject,
            spawnPos,
            Quaternion.identity,
            spawnedVehiclesContainer
        );

        lastLaneSpawned = hit.collider;
        currentTimeUntilNextSpawn = timeUntilNextSpawn;

        // Reduce Car Wallet Count
        carWallet.carCount--;

        // Reduce Player Money
        if (currentActiveCar.carPrice > 0 && !isForWorldSelect)
        {
            gameManager.UpdateTokens(currentActiveCar.carPrice * -1);
            soundManager.PlayPurchase();
        }   

        if(currentActiveCar.isUltimate)
            ultimate.isReady = false;

        if (selectDefaultOnPlace)
            SelectCar(standardCar);
    }

    private IEnumerator WaitAndEnableSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        disableVehicleSpawn = false;
    }

    public bool NotEnoughCarWallet()
    {
        return carWallet.carCount <= 0;
    }

    public void SelectCar(Car car)
    {
        if (car.carPrice <= gameManager.tokens)
        {
            currentActiveCar = car;
        }
        else
        {
            soundManager.PlayCantPurchase();
        } 
    }

    private void UpdateCarCursor()
    {
        if (currentCarIndicator != null)
        {
            if (currentCarIndicator.followCursor)
                currentCarIndicator.transform.position = new Vector3(inputPos.x, inputPos.y, 0);
            currentCarIndicator.SetUI(currentActiveCar, gameManager.tokens, carWallet.carCount);
        }
    }

    private bool IsMouseOverUIElement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
        if (hit.collider != null && hit.collider.GetComponent<GraphicRaycaster>() != null)
            return true;
        return false;
    }

    private void DecreaseSpawnTimer()
    {
        currentTimeUntilNextSpawn -= Time.deltaTime;
    }
}
