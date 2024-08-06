using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{

    
    public static void TriggerAchievement(string achievementName){
        if(SteamManager.Initialized) {
            if(SteamManager.Initialized)
            {
                Steamworks.SteamUserStats.GetAchievement(achievementName, out bool achievementCompleted);

                if(!achievementCompleted){
                    SteamUserStats.SetAchievement(achievementName);
                    SteamUserStats.StoreStats();
                    Debug.Log("Achievement Completed! "+ achievementName);
                }
                else{
                    Debug.Log("Achievement Already Completed!");
                }
            }
            else{
                Debug.Log("we are not connected");
            }
        }
    }
}
