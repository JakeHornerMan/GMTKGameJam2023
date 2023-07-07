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

        if (hit.collider == null) return;

        if (hit.collider.gameObject != gameObject) return;

        bool mouseUp = mousePosition.y > spawnZoneDivider;
        topHighlight.SetActive(mouseUp);
        bottomHighlight.SetActive(!mouseUp);
    }
}
