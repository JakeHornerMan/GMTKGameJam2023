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

    public List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    public List<LeaderboardEntry> top10LeaderboardEntries = new List<LeaderboardEntry>();
    public List<LeaderboardEntry> friendsLeaderboardEntries = new List<LeaderboardEntry>();

    private void Awake() 
    {
        if(SteamManager.Initialized) {
            SteamLeaderboards.Init();
		}
    }

    private void Start()
    {
        GatherLeaderboardValues();
    }

    // // Clear all buttons in grid layout container
    private void ClearGrid()
    {
        for (var i = entriesContainer.transform.childCount - 1; i >= 0; i--)
            Destroy(entriesContainer.transform.GetChild(i).gameObject);
    }

    private void GatherLeaderboardValues()
    {
        SteamLeaderboards.DownloadScoresAroundUser();
        leaderboardEntries = SteamLeaderboards.leaderboardEntries;
        SteamLeaderboards.DownloadScoresTop();
        top10LeaderboardEntries = SteamLeaderboards.leaderboardEntries;
        SteamLeaderboards.DownloadScoresForFriends();
        friendsLeaderboardEntries = SteamLeaderboards.leaderboardEntries;
        if(leaderboardEntries.Count > 0) ShowLeaderboard();
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
        currentViewType = ViewType.Leaderboard;
        foreach (var entry in leaderboardEntries)
        {
            entry.ToString();
        }
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
        currentViewType = ViewType.Top10;
        foreach (var entry in top10LeaderboardEntries)
        {
            entry.ToString();
        }
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
        currentViewType = ViewType.Friends;
        foreach (var entry in friendsLeaderboardEntries)
        {
            entry.ToString();
        }
    }
}