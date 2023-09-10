using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("Camera Keyboard Control")]
    [SerializeField] private float cameraScrollSpeed = 1f;

    [Header("Keybindings")]
    [SerializeField] private KeyCode upKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode downKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;

    private Camera cam;
    private Transform camTransform;

    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = Camera.main;
        camTransform = cam.transform;
    }

    private void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        // Mouse position when drag starts (1st click)
        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        // If held down, calculate distance from drag origin to endpoint & Move camera
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            camTransform.position += difference;
        }
    }
}
