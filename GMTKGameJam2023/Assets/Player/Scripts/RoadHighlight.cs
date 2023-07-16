using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHighlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bottomHighlight;

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
            bottomHighlight.SetActive(false);
            return;
        }

        bool touchingRoad = hit.collider.gameObject == gameObject;

        bottomHighlight.SetActive(touchingRoad);
    }
}
