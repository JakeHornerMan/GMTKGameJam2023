using System;
using System.Collections;
using UnityEngine;

public class WorldSelectScroll : MonoBehaviour
{
    [Header("Camera Control")]
    [SerializeField] private float cameraScrollSpeed = 1f;

    [Header("Keybindings")]
    [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;

    private Transform mainCameraTransform;

    private void Awake()
    {
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleKeyboardScroll();
    }

    private void HandleKeyboardScroll()
    {
        Vector3 camPos = mainCameraTransform.position;
        if (Input.GetKey(rightKey))
            camPos.x += cameraScrollSpeed;
        else if (Input.GetKey(leftKey))
            camPos.x -= cameraScrollSpeed;
        mainCameraTransform.position = camPos;
    }
}
