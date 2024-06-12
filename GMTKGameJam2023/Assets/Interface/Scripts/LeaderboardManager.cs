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
    [SerializeField] private GameObject leaderboardEntryPrefab;

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
    private void ClearEntries()
    {
        for (var i = entriesContainer.transform.childCount - 1; i >= 0; i--)
            Destroy(entriesContainer.transform.GetChild(i).gameObject);
    }

    private void GatherLeaderboardValues()
    {
        SteamLeaderboards.DownloadScoresAroundUser();
        leaderboardEntries = SteamLeaderboards.leaderboardEntries;
        SteamLeaderboards.DownloadScoresTop();
        top10LeaderboardEntries = SteamLeaderboards.top10LeaderboardEntries;
        SteamLeaderboards.DownloadScoresForFriends();
        friendsLeaderboardEntries = SteamLeaderboards.friendsLeaderboardEntries;
        if(leaderboardEntries.Count > 0) ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        Debug.Log("ShowLeaderboard");
        ClearEntries();
        currentViewType = ViewType.Leaderboard;
        foreach (LeaderboardEntry entry in leaderboardEntries)
        {
            entry.Log();
            LeaderboardEntryVisual obj = Instantiate(
                leaderboardEntryPrefab,
                entriesContainer
            ).GetComponent<LeaderboardEntryVisual>();
            obj.SetValues(entry);
        }
    }

    public void ShowTop10()
    {
        Debug.Log("ShowTop10");
        ClearEntries();
        currentViewType = ViewType.Top10;
        foreach (var entry in top10LeaderboardEntries)
        {
            entry.Log();
            LeaderboardEntryVisual obj = Instantiate(
                leaderboardEntryPrefab,
                entriesContainer
            ).GetComponent<LeaderboardEntryVisual>();
            obj.SetValues(entry);
        }
    }

    public void ShowFriends()
    {
        Debug.Log("ShowFriends");
        ClearEntries();
        currentViewType = ViewType.Friends;
        foreach (var entry in friendsLeaderboardEntries)
        {
            entry.Log();
            LeaderboardEntryVisual obj = Instantiate(
                leaderboardEntryPrefab,
                entriesContainer
            ).GetComponent<LeaderboardEntryVisual>();
            obj.SetValues(entry);
        }
    }
}