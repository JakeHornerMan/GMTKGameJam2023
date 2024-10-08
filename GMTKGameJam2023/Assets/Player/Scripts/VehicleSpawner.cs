using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public Car currentActiveCar;
    public Ultimate currentUltimateAbility;

    private float currentTimeUntilNextSpawn;

    private Collider2D lastLaneSpawned;
    private CurrentCarIndicator currentCarIndicator;
    private Camera mainCamera;
    private GameManager gameManager;
    private TutorialManager tutorialManager;
    private UltimateManager ultimateManager;
    private SoundManager soundManager;
    private CarWallet carWallet;
    private CameraShaker cameraShaker;
    private TokenSpawner tokenSpawner;

    public Vector3 inputPos;

    [HideInInspector] public List<CarButton> carButtons;

    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        ultimateManager = FindObjectOfType<UltimateManager>();
        soundManager = FindObjectOfType<SoundManager>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        carWallet = GetComponent<CarWallet>();
        currentCarIndicator = FindObjectOfType<CurrentCarIndicator>();
        tokenSpawner = FindObjectOfType<TokenSpawner>();
    }

    private void Start()
    {
        if (gameManager != null)
        {
            CreateButtons(gameManager.carsInLevel);
        }
        if (tutorialManager != null)
        {
            CreateButtons(tutorialManager.carsInLevel);
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.isGameOver) return;
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

    public void CreateButtons(List<Car> carsInLevel)
    {

        foreach (Car car in carsInLevel)
        {
            CarButton btn = Instantiate(
                carButtonPrefab,
                carSelectContainer
            ).GetComponent<CarButton>();
            carButtons.Add(btn);
            btn.correspondingCar = car;
        }

        // Old implementation, when standard car used to be part of normal inventory
        // if (carButtons.Count >= 1)
        //     currentActiveCar = standardCar;

        SetStandardCar();
    }

    public void DestroyButtons()
    {
        foreach (Transform child in carSelectContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void MouseInputs()
    {
        if (Input.GetMouseButtonDown(placeMouseBtn))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (currentUltimateAbility)
            {
                PlaceSelectedUltimate();
            }
            else
            {
                PlaceSelectedCar();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && currentUltimateAbility != null)
        {
            if(!ultimateManager.ultimateEnabled) return;
            // SelectCar(standardCar);
            SelectUltimate(currentUltimateAbility);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && standardCar != null)
        {
            SelectCar(standardCar);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q) && carButtons.Count >= 1)
            SetStandardCar();

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.W) && carButtons.Count >= 2)
            SelectCar(carButtons[0].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.E) && carButtons.Count >= 3)
            SelectCar(carButtons[1].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.R) && carButtons.Count >= 4)
            SelectCar(carButtons[2].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.T) && carButtons.Count >= 5)
            SelectCar(carButtons[3].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Y) && carButtons.Count >= 6)
            SelectCar(carButtons[4].correspondingCar);

        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.U) && carButtons.Count >= 7)
            SelectCar(carButtons[5].correspondingCar);

        // if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.P))
        //     SelectUltimate(ultimateManager.correspondingUltimate);

        UpdateMousePos();

        if (gameManager != null)
        {
            UpdateCarCursor(gameManager.tokens);
        }
        if (tutorialManager != null)
        {
            UpdateCarCursor(tutorialManager.tokens);
        }
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
                    if (currentUltimateAbility)
                    {
                        PlaceSelectedUltimate();
                    }
                    else
                    {
                        PlaceSelectedCar();
                    }
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

    public void SetStandardCar()
    {
        SelectCar(standardCar);
    }

    private void PlaceSelectedCar()
    {
        if(gameManager != null && gameManager.isGameOver) return; 
        if(tutorialManager != null && tutorialManager.isGameOver) return;
        // Check Money, Check Car Wallet Budget
        if (NotEnoughCarWallet())
            return;

        // Raycast toward Click
        // RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero);

        // Define the LayerMask to ignore the "Cement" layer.
        int cementLayer = LayerMask.NameToLayer("Cement"); // Get the layer index for "Cement".
        int layerMask = ~(1 << cementLayer); // Create a mask that ignores the "Cement" layer.

        LayerMask mask = LayerMask.GetMask("Ignore Raycast", "Cement");

        // Perform the raycast while ignoring the "Cement" layer.
        RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero, Mathf.Infinity, ~mask);

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
            CameraShaker.instance.Shake(invalidPlacementCamShake.x, invalidPlacementCamShake.y);
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
        if (gameManager != null) gameManager.vehicleSpawnCount++;

        // Reduce Player Money
        if (currentActiveCar.carPrice > 0 && !isForWorldSelect)
        {
            if (gameManager != null) gameManager.UpdateTokens(currentActiveCar.carPrice * -1);
            if (tutorialManager != null) tutorialManager.UpdateTokens(currentActiveCar.carPrice * -1);
            soundManager.PlayPurchase();
        }

        if (selectDefaultOnPlace)
            SelectCar(standardCar, false);
    }

    private void PlaceSelectedUltimate()
    {
        if(gameManager != null && gameManager.isGameOver) return; 
        if(tutorialManager != null && tutorialManager.isGameOver) return;
        // Raycast toward Click
        RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero);

        // Return if Clicked Nothing
        if (hit.collider == null)
            return;

        if (IsMouseOverUIElement())
            return;

        // Spawn Car at Road at Position
        Vector3 spawnPos;
        if (currentUltimateAbility.placeableAnywhere)
            spawnPos = new Vector3(inputPos.x, inputPos.y, 1);
        else
            spawnPos = hit.collider.transform.position + (Vector3)spawnOffset;

        // To prevent car spamming on the same lane
        if (hit.collider == lastLaneSpawned && currentTimeUntilNextSpawn > 0)
            return;

        Instantiate(
            currentUltimateAbility.gameObject,
            spawnPos,
            Quaternion.identity,
            spawnedVehiclesContainer
        );

        ultimateManager.ultimateEnabled = false;

        lastLaneSpawned = hit.collider;
        currentTimeUntilNextSpawn = timeUntilNextSpawn;

        // // Reduce Car Wallet Count
        // carWallet.carCount--;

        if (gameManager != null) gameManager.ultimateSpawnCount++;

        if (selectDefaultOnPlace)
        {
            SelectCar(standardCar, false);
            SelectUltimate(null);
        }
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

    public void SelectCar(Car car, bool shineLane = true)
    {
        if (gameManager != null)
        {
            SetActiveCar(car, shineLane, gameManager.tokens);
            SelectUltimate(null);
        }
        if (tutorialManager != null)
        {
            SetActiveCar(car, shineLane, tutorialManager.tokens);
            SelectUltimate(null);
        }
    }

    public void SetActiveCar(Car car, bool shineLane, int tokens)
    {
        if (car.carPrice <= tokens)
        {
            currentActiveCar = car;

            if (!shineLane)
            {
                return;
            }
            foreach (GameObject lane in tokenSpawner.allLanes)
            {
                RoadHighlight laneHighlight = lane.GetComponent<RoadHighlight>();
                laneHighlight.DisableShineObject();
                if (currentActiveCar.placeableLaneTags.Contains(laneHighlight.gameObject.tag))
                {
                    if (Settings.RoadShineEnabled)
                        laneHighlight.ShineLane();
                }
            }
        }
        else
        {
            soundManager.PlayCantPurchase();
        }
    }

    public void SetUltimate()
    {
        if (ultimateManager.ultimateEnabled)
        {
            SelectUltimate(ultimateManager.correspondingUltimate);
        }
    }

    public void SelectUltimate(Ultimate ultimate)
    {
        currentUltimateAbility = ultimate;
        // Debug.Log(currentUltimateAbility.gameObject.name);
    }

    private void UpdateCarCursor(int tokens)
    {
        if (currentCarIndicator != null)
        {
            if (currentCarIndicator.followCursor)
                currentCarIndicator.transform.position = new Vector3(inputPos.x, inputPos.y, 0);

            if (currentUltimateAbility != null)
                currentCarIndicator.SetUI(currentUltimateAbility);
            else
                currentCarIndicator.SetUI(currentActiveCar, tokens, carWallet.carCount);
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
