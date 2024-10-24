using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Steam : MonoBehaviour
{

    public int score = 1;
    public bool devMode = false;

    public void Awake() {
		if(SteamManager.Initialized) {
			string name = SteamFriends.GetPersonaName();
			Debug.Log("Steam Account: " + name);
            SteamLeaderboards.Init();
            resetAchievements();
		}
	}

	void Update()
    {
        if(!SteamManager.Initialized)
        {
            Debug.Log("we are not connected");
            return;
        }

        if(devMode){
            // if (Input.GetKeyDown(KeyCode.LeftShift))
            // {
            //     score++;
            // }

            // if (Input.GetKeyDown(KeyCode.D))
            // {
            //     SteamLeaderboards.DownloadScoresTop();
            // }

            // if (Input.GetKeyDown(KeyCode.F))
            // {
            //     SteamLeaderboards.DownloadScoresAroundUser();
            // }

            // if (Input.GetKeyDown(KeyCode.G))
            // {
            //     SteamLeaderboards.DownloadScoresForFriends();
            // }

            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     UploadLeaderboard(score);
            // }

            // if (Input.GetKeyDown(KeyCode.Home))
            // {
            //     SetAchievement();
            // }

            // if (Input.GetKeyDown(KeyCode.PageUp))
            // {
            //     SetStat();
            // }

            // if (Input.GetKeyDown(KeyCode.PageDown))
            // {
            //     GetStat();
            // }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Debug.Log("Reset All Stats & Achievements!");
                resetAchievements();
            }
        }
    }

    public void UploadLeaderboard(int score = 1, int round = 0)
    {
        if(SteamManager.Initialized)
        {
            // SteamLeaderboards.Init();
            SteamLeaderboards.UpdateScore(score, round);
        }
    }

    public int GetStat(string statName = "TEST_STAT"){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat(statName, out int count);
            Debug.Log(statName + " Stat Count: " + count);
            SteamUserStats.StoreStats();
            return count;
        }
        else{
            Debug.Log("Counld not All Connect for Stat!");
            return 0;
        }
    }

    public void SetStat(string statName = "TEST_STAT"){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat(statName, out int count);
            Debug.Log(statName + " Before Count: " + count);
            count++;
            
            Steamworks.SteamUserStats.SetStat(statName, count);
            Debug.Log(statName + " After Count: " + count);
            SteamUserStats.StoreStats();
        }
    }

    public void SetAchievement(string achievementName = "TEST_ACHIEVEMENT"){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement(achievementName, out bool achievementCompleted);

            if(!achievementCompleted){
                SteamUserStats.SetAchievement(achievementName);
                SteamUserStats.StoreStats();
                Debug.Log("Achievement Completed!");
            }
            else{
                Debug.Log("Achievement Already Completed!");
                return;
            }
        }
        else{
            Debug.Log("we are not connected");
        }
    }

    void resetAchievements(){
        if(SteamManager.Initialized)
        {
            // SteamUserStats.ResetAllStats(true);
            Debug.Log("Reset all stats");
        }
    }
}
