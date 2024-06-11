using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles spawning of buttons in a grid for all
/// vehicles, ultimates, and chicken.
/// </summary>
public class LeaderboardManager : MonoBehaviour
{
    public enum ViewType { Leaderboard, Top10, Friends }
    public ViewType currentViewType;
    [SerializeField] private Transform entriesContainer;

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private List<LeaderboardEntry> top10LeaderboardEntries = new List<LeaderboardEntry>();
    private List<LeaderboardEntry> friendsLeaderboardEntries = new List<LeaderboardEntry>();

    private void Start()
    {
        GatherLeaderboardValues();
        ShowLeaderboard();
    }

    // // Clear all buttons in grid layout container
    private void ClearGrid()
    {
        for (var i = entriesContainer.transform.childCount - 1; i >= 0; i--)
            Destroy(buttonsContainer.transform.GetChild(i).gameObject);
    }

    private void GatherLeaderboardValues()
    {
        
    }

    // // Run from UI by button
    // public void ShowCarButtons()
    // {
    //     currentViewType = ViewType.Cars;
    //     ClearGrid();
    //     foreach (Car car in carsToShow)
    //     {
    //         GuidebookButton btn = Instantiate(
    //             guideSelectButtonPrefab,
    //             buttonsContainer
    //         ).GetComponent<GuidebookButton>();
    //         btn.Creation(car);
    //     }
    // }
    public void ShowLeaderboard()
    {
        Debug.Log("ShowLeaderboard");
    }

    // public void ShowChickenButtons()
    // {
    //     currentViewType = ViewType.Chicken;
    //     ClearGrid();
    //     foreach (ObjectInfo chicken in chickenToShow)
    //     {
    //         GuidebookButton btn = Instantiate(
    //             guideSelectButtonPrefab,
    //             buttonsContainer
    //         ).GetComponent<GuidebookButton>();
    //         btn.Creation(chicken);
    //     }
    // }
    public void ShowTop10()
    {
        Debug.Log("ShowTop10");
    }

    // public void ShowUltimateButtons()
    // {
    //     currentViewType = ViewType.Ultimates;
    //     ClearGrid();
    //     foreach (Ultimate ultimate in ultimatesToShow)
    //     {
    //         GuidebookButton btn = Instantiate(
    //             guideSelectButtonPrefab,
    //             buttonsContainer
    //         ).GetComponent<GuidebookButton>();
    //         btn.Creation(ultimate);
    //     }
    // }

    public void ShowFriends()
    {
        Debug.Log("ShowFriends");
    }
}