using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMenuFunction : MonoBehaviour
{
    [SerializeField] public string option;
    private CameraMenuMove mainCamera;
    private CameraShaker cameraShaker;

    // private Vector3 targetPos = new Vector3(0, -10, 0);
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
            // code block
            break;
        case "GoToCredits":
            // code block
            break;
        default:
            break;
        }  
    }

    public void GoToGameSelect(){
        mainCamera.targetPos = new Vector3(0, 20, -10);
    }

    public void GoToSettings(){

    }

    public void GoToCredits(){

    }

    public void GoToStart(){
        mainCamera.targetPos = new Vector3(0, 0, -10);
    }
}
