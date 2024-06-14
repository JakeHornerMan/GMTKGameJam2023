using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    // private SteamScript steamScript;
    private LeaderboardManager leaderboardManager;
    void Awake()
    {
        // steamScript = FindObjectOfType<SteamScript>();
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
    }

    void Start()
    {
        leaderboardManager.UploadToLeaderboardAndUpdate(Points.playerScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
