using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteraction : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private GameObject activeCar;
    [SerializeField] private int placeMouseBtn = 0;
    [SerializeField] private Vector2 spawnOffset = new(0, -5);

    [Header("References")]
    [SerializeField] private Vector2 invalidPlacementCamShake = new(0.15f, 0.2f);
    private Camera mainCamera;
    private Vector3 inputPos;
    private CameraShaker cameraShaker;
    public bool canPlace = true;
    public bool carPlaced = false;
    public GameObject pointer;
    [SerializeField] private LayerMask roadLayer;

    void Start()
    {
        mainCamera = Camera.main;
        cameraShaker = FindObjectOfType<CameraShaker>();
        pointer.SetActive(false);
        StartCoroutine(PlayPointer());
    }

    void Update()
    {
        
        if (SystemInfo.deviceType == DeviceType.Desktop)
            MouseInputs();

        if (SystemInfo.deviceType == DeviceType.Handheld)
            TouchInputs();
    }

    private void MouseInputs()
    {
        if (Input.GetMouseButtonDown(placeMouseBtn))
            if(canPlace){
                PlaceSelectedCar();
            }

        UpdateMousePos();
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
                    if(canPlace){
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

    private void PlaceSelectedCar()
    {
        // Raycast toward Click
        RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero, 1000f, roadLayer);

        // Return if Clicked Nothing
        if (hit.collider == null)
            return;

        // if (IsMouseOverUIElement())
        //     return;

        // Return if Car Cannot be Placed on Clicked Lane
        // if (isForWorldSelect)
        // {
        //     if (hit.collider.GetComponent<WorldSelectLaneCar>() == null) return;
        //     currentActiveCar = hit.collider.GetComponent<WorldSelectLaneCar>().worldSelectCar;
        // }

        // Spawn Car at Road at Position
        Vector3 spawnPos;
        // if (activeCar.placeableAnywhere)
            // spawnPos = new Vector3(inputPos.x, inputPos.y, 1);
        // else
            spawnPos = hit.collider.transform.position + (Vector3)spawnOffset;

        // To prevent car spamming on the same lane
        // if (hit.collider == lastLaneSpawned && currentTimeUntilNextSpawn > 0)
        //     return;

        Instantiate(
            activeCar.gameObject,
            spawnPos,
            Quaternion.identity
        );

        carPlaced = true;
        if(pointer.activeSelf)
            pointer.GetComponent<Animator>().Play("ClickDisappear");

        StartCoroutine(CannotPlace());


        // lastLaneSpawned = hit.collider;
        // currentTimeUntilNextSpawn = timeUntilNextSpawn;

    }

    public IEnumerator CannotPlace()
    {
        canPlace= false;
        yield return new WaitForSeconds(3f);
        canPlace= true;
    }

    public IEnumerator PlayPointer()
    {
        yield return new WaitForSeconds(5f);
        if(!carPlaced)
            pointer.SetActive(true);
    }

    // private bool IsMouseOverUIElement()
    // {
    //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
    //     if (hit.collider != null && hit.collider.GetComponent<GraphicRaycaster>() != null)
    //         return true;
    //     return false;
    // }
}
