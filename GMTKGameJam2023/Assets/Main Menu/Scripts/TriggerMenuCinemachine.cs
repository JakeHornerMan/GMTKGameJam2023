using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMenuCinemachine : MonoBehaviour
{

    public static TriggerMenuCinemachine instance;
    public LeaderboardManager leaderboardManager;

    public MenuRoad currentRoad;

    //[SerializeField] public string option;
    [SerializeField] private Animator anim;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject settings;

    [Header("Option-Specific Objects")]
    [SerializeField] private GameObject carVeerLeft;
    private Vector3 carVeerLeftOrigin;

    [SerializeField] private GameObject carVeerRight;
    private Vector3 carVeerRightOrigin;

    // private float speed = 3f;

    private void Start()
    {
        leaderboardManager = FindObjectOfType<LeaderboardManager>();

        if (instance == null)
        {
            instance = this;
        }


        carVeerLeft.SetActive(false);
        carVeerRight.SetActive(false);

        SetAllSpecialOrigins();
        ResetAllSpecialObjects();
    }

    private void SetAllSpecialOrigins()
    {
        carVeerLeftOrigin = carVeerLeft.transform.position;
        carVeerRightOrigin = carVeerRight.transform.position;
    }

    private void ResetAllSpecialObjects()
    {
        carVeerLeft.transform.position = carVeerLeftOrigin;
        carVeerLeft.SetActive(false);

        carVeerRight.transform.position = carVeerRightOrigin;
        carVeerRight.SetActive(false);
    }

    public void LaneSelect(MenuRoad road)
    {
        currentRoad = road;

        string option = currentRoad.roadName;

        foreach (MenuRoad menuRoad in FindObjectsOfType<MenuRoad>())
        {
            menuRoad.ResetRoad();

        }

        ResetAllSpecialObjects();

        MenuManager.instance.ShowBackButton();

        switch (option)
        {
            case "Play":
                Debug.Log(option);
                GoToGameSelect();
                break;
            case "Start":
                Debug.Log(option);
                GoToStart();
                break;
            case "Settings":
                Debug.Log(option);
                GoToSettings();
                break;
            case "Credits":
                Debug.Log(option);
                GoToCredits();
                break;
            case "Begin":
                Debug.Log(option);
                GoToBeginGame();
                break;
            case "Collection":
                Debug.Log(option);
                GoToCollection();
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

    public void GoToGameSelect()
    {
        MenuInteraction.instance.PlaceSelectedCar(currentRoad);
        anim.Play("Play");
    }

    public void GoToSettings()
    {
        // isSettings = true;
        carVeerLeft.SetActive(true);
        anim.Play("Settings");
    }

    public void GoToCredits()
    {
        carVeerRight.SetActive(true);
        anim.Play("Credits");
        leaderboardManager.SetLeaderboardList();
    }

    public void GoToStart()
    {
        // isSettings = false;
        MenuManager.instance.HideBackButton();
        anim.Play("Start");
    }

    public void GoToBeginGame()
    {
        MenuInteraction.instance.PlaceSelectedCar(currentRoad);
        anim.Play("Enter Game");

        GameProgressionValues.SetDefaultValues();
        PlayerValues.SetDefaultValues();
        Points.SetDefaultValues();

        MenuManager.instance.EnterMainMenuScene();
        // mainMenu.EnterGame();
    }

    public void GoToCollection()
    {
        MenuInteraction.instance.PlaceSelectedCar(currentRoad);
        anim.Play("Collection");

        MenuManager.instance.EnterScene("Guidebook");

    }

    public void GoToTutorial()
    {
        MenuInteraction.instance.PlaceSelectedCar(currentRoad);
        anim.Play("Tutorial");

        MenuManager.instance.EnterScene("Tutorial");
    }
}
