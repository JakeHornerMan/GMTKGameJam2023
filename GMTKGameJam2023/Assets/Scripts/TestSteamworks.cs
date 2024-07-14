using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class TestSteamworks : MonoBehaviour
{
    // protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    void Start() {
		if(SteamManager.Initialized) {
			string name = SteamFriends.GetPersonaName();
			Debug.Log(name);
		}
	}

    // private void OnEnable() {
	// 	if (SteamManager.Initialized) {
	// 		m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
	// 	}
	// }

    // private void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
	// 	if(pCallback.m_bActive != 0) {
	// 		Debug.Log("Steam Overlay has been activated");
	// 	}
	// 	else {
	// 		Debug.Log("Steam Overlay has been closed");
	// 	}
	// }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("test triggerd!!!");
            TestStat();
            TestAchievement();
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            Debug.Log("Reset Achievement!");
            // resetAchievements();
        }
    }

    public void TestStat(){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat("TEST_STAT", out int count);
            Debug.Log("Before Count: " + count);
            count++;
            
            Steamworks.SteamUserStats.SetStat("TEST_STAT", count);
            Debug.Log("After Count: " + count);
            SteamUserStats.StoreStats();
        }
        else{
            Debug.Log("we are not connected");
        }
    }

    public void TestAchievement(){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement("TEST_ACHIEVEMENT", out bool achievementCompleted);

            if(!achievementCompleted){
                SteamUserStats.SetAchievement("TEST_ACHIEVEMENT");
                SteamUserStats.StoreStats();
                Debug.Log("Achievement Completed!");
            }
            else{
                Debug.Log("Achievement Already Completed!");
            }
        }
        else{
            Debug.Log("we are not connected");
        }
    }

    void resetAchievements(){
        SteamUserStats.ResetAllStats(true);
        Debug.Log("Reset all stats");
    }
}
