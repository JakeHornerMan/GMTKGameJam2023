using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject topPanel;
    [SerializeField] private GameObject bottomPanel;

    [Header("Gameplay Values")]
    [SerializeField] private float spawnZoneDivider = 0;

    private Camera mainCamera;

    public HoverPos mouseHoverPos = HoverPos.Top;

    public enum HoverPos
    {
        Top,
        Bottom,
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        SetMousePos();
        HandleHoverIndicators();
    }

    private void HandleHoverIndicators()
    {
        topPanel.SetActive(mouseHoverPos == HoverPos.Top);
        bottomPanel.SetActive(mouseHoverPos == HoverPos.Bottom);
    }

    private void SetMousePos()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseHoverPos = (mousePosition.y > spawnZoneDivider) ? HoverPos.Top : HoverPos.Bottom;
    }
}
