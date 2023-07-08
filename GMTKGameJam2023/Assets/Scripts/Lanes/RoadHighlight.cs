using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHighlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject topHighlight;
    [SerializeField] private GameObject bottomHighlight;

    [Header("Spawn Positioning")]
    [SerializeField] private float spawnZoneDivider = 0;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider == null)
        {
            topHighlight.SetActive(false);
            bottomHighlight.SetActive(false);
            return;
        };

        bool touchingRoad = hit.collider.gameObject == gameObject;
        bool mouseUp = mousePosition.y > spawnZoneDivider;

        topHighlight.SetActive(mouseUp && touchingRoad);
        bottomHighlight.SetActive(!mouseUp && touchingRoad);
    }
}
