using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PixelPerfectCameraCustom : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float referenceResolutionHeight;
    //[SerializeField] private float intendedOrthographic;

    [SerializeField] private float CameraScale;
    [SerializeField] private float PPU = 32f;

    // Start is called before the first frame update
    void Start()
    {

        //mainCamera.orthographicSize = ((Screen.currentResolution.height) / (CameraScale * 32)) * 0.5f;

        //float intendedScale = editorsScreenHeight / (2 * intendedOrthographic * PPU);

        //int scaleInt = Mathf.RoundToInt(intendedScale);

        // Define your reference resolution height
        //referenceResolutionHeight = 1080f;

        // Calculate the ratio
        float resolutionRatio = Screen.currentResolution.height / referenceResolutionHeight;

        mainCamera.orthographicSize = ((Screen.currentResolution.height / resolutionRatio) / (CameraScale * PPU)) * 0.5f;
    }

    
}
