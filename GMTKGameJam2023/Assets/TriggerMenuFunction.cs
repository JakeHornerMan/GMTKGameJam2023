using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMenuFunction : MonoBehaviour
{
    [SerializeField] public string option;
    private CameraMenuMove mainCamera;
    private CameraShaker cameraShaker;

    private Vector3 startPos = new Vector3(0, -0, -10);
    private Vector3 gamesPos = new Vector3(0, 20, -10);
    private Vector3 settingsPos = new Vector3(10, 0, -10);
    private Vector3 creditsPos = new Vector3(10, 0, -10);


    // private float speed = 3f;

    private void Start()
    {
        mainCamera = FindObjectOfType<CameraMenuMove>();
        cameraShaker = FindObjectOfType<CameraShaker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<Car>()) return;

        other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        switch(option) 
        {
        case "GoToGameSelect":
            GoToGameSelect();
            break;
        case "GoToStart":
            GoToStart();
            break;
        case "GoToSettings":
            GoToSettings();
            break;
        case "GoToCredits":
            GoToCredits();
            break;
        default:
            break;
        }  
    }

    public void GoToGameSelect(){
        mainCamera.targetPos = gamesPos;
    }

    public void GoToSettings(){
        mainCamera.targetPos = settingsPos;
    }

    public void GoToCredits(){
        mainCamera.targetPos = creditsPos;
    }

    public void GoToStart(){
        mainCamera.targetPos = startPos;
    }
}
