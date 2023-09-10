using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{

    public Camera mainCamera;
    public float zoomSpeed = 0.1f;
    public float targetZoom = 2f;

    [SerializeField] private GameObject cloudController;


    // Start is called before the first frame update
    void Start()
    {
        // You can find and assign the main camera like this, or drag and drop it in the editor.
        mainCamera = Camera.main;
    }

    public void ZoomOnWorld(Button button)
    {
        // Get the position of the button
        Vector3 buttonPosition = button.transform.position;

        buttonPosition.z = -10;

        // Start zoom coroutine
        StartCoroutine(ZoomCamera(buttonPosition));

        cloudController.SetActive(true);

    }

    private IEnumerator ZoomCamera(Vector3 targetPosition)
    {
        Vector3 initialPosition = mainCamera.transform.position;
        float initialZoom = mainCamera.orthographicSize;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;

            // Linearly interpolate camera position
            mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Linearly interpolate zoom level
            mainCamera.orthographicSize = Mathf.Lerp(initialZoom, targetZoom, t);

            yield return null;
        }
    }
}
