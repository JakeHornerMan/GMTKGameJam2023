using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Steam : MonoBehaviour
{
    void Start() {
		if(SteamManager.Initialized) {
			string name = SteamFriends.GetPersonaName();
			Debug.Log("Steam Account: " + name);
		}
	}

	void Update()
    {
        if(!SteamManager.Initialized)
        {
            Debug.Log("we are not connected");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            SetAchievement();
        }

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            SetStat();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            GetStat();
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            Debug.Log("Reset All Stats & Achievements!");
            resetAchievements();
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
            SteamUserStats.ResetAllStats(true);
            Debug.Log("Reset all stats");
        }
    }
}
