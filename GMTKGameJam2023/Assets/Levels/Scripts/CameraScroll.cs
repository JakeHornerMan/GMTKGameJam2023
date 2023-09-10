using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("Camera Keyboard Control")]
    [SerializeField] private float cameraScrollSpeed = 1f;

    [Header("Keybindings (Arrow Keys)")]
    [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;

    [Header("Keybindings (WASD Keys)")]
    [SerializeField] private KeyCode rightKeyAlt = KeyCode.D;
    [SerializeField] private KeyCode leftKeyAlt = KeyCode.A;

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
        // Keyboard movement
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(rightKey) || Input.GetKey(rightKeyAlt))
            moveDirection += Vector3.right;
        if (Input.GetKey(leftKey) || Input.GetKey(leftKeyAlt))
            moveDirection += Vector3.left;
        moveDirection.Normalize();
        camTransform.position += moveDirection * cameraScrollSpeed * Time.deltaTime;

        // Mouse controls
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            difference = new Vector3(difference.x, 0, 0);
            camTransform.position += difference;
        }
    }
}
