using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMenuFunction : MonoBehaviour
{
    [SerializeField] public string option;
    private CameraMenuMove mainCamera;
    private CameraShaker cameraShaker;
    private MainMenu mainMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject settings;

    private Vector3 startPos = new Vector3(0, -0, -10);
    private Vector3 gamesPos = new Vector3(0, 20, -10);
    private Vector3 settingsPos = new Vector3(-30, 0, -10);
    private Vector3 creditsPos = new Vector3(30, 0, -10);


    // private float speed = 3f;

    private void Start()
    {
        mainCamera = FindObjectOfType<CameraMenuMove>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        mainMenu = FindObjectOfType<MainMenu>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<Car>()) return;

        // Debug.Log("This was hit");
        other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        switch(option) 
        {
        case "GoToGameSelect":
            Debug.Log(option);
            GoToGameSelect();
            break;
        case "GoToStart":
            Debug.Log(option);
            GoToStart();
            break;
        case "GoToSettings":
            Debug.Log(option);
            GoToSettings();
            break;
        case "GoToCredits":
            Debug.Log(option);
            GoToCredits();
            break;
        case "PlayGame":
            Debug.Log(option);
            GoToGame();
            break;
        case "Bestiary":
            Debug.Log(option);
            GoToBeastiary();
            break;
        case "Tutorial":
            Debug.Log(option);
            GoToTutorial();
            break;
        default:
            Debug.Log("There is nothing set");
            break;
        }  
    }

    public void GoToGameSelect(){
        mainCamera.speed = 5f;
        mainCamera.targetPos = gamesPos;
    }

    public void GoToSettings(){
        mainCamera.speed = 12f;
        mainCamera.targetPos = settingsPos;
    }

    public void GoToCredits(){
        mainCamera.speed = 12f;
        mainCamera.targetPos = creditsPos;
        credits.SetActive(true);
    }

    public void GoToStart(){
        mainCamera.targetPos = startPos;
        credits.SetActive(false);
    }

    public void GoToGame(){
        mainCamera.speed = 5f;
        mainCamera.targetPos = gamesPos;
        mainMenu.EnterGame();
    }

    public void GoToBeastiary(){
        // mainCamera.speed = 5f;
        // mainCamera.targetPos = gamesPos;
        // mainMenu.EnterWorldSelect();
    }

    public void GoToTutorial(){
        // mainCamera.speed = 5f;
        // mainCamera.targetPos = gamesPos;
        // mainMenu.EnterWorldSelect();
    }
}
