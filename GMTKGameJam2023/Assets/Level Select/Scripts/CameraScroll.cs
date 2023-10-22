using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{

    [Header("Scrolling Restrictions")]
    [SerializeField] private float minX = -0.35f;
    [SerializeField] private float maxX = 100f;

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

        // Calculate the new camera position
        Vector3 newPosition = camTransform.position + moveDirection * cameraScrollSpeed * Time.deltaTime;

        // Clamp the x position to stay within the specified range
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Update the camera's position
        camTransform.position = newPosition;

        // Mouse controls
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            difference = new Vector3(difference.x, 0, 0);

            // Calculate the new camera position when dragging
            newPosition = camTransform.position + difference;

            // Clamp the x position to stay within the specified range
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            // Update the camera's position
            camTransform.position = newPosition;
        }
    }
}
