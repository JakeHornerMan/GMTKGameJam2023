using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    //create a constant in all cap on what you want to name the achievement
    private const string test_achievementName = "TEST_ACHIEVEMENT";
    
    //create a static function like so that passes though your constant
    //we can call this using SteamAchievements.TestAchievement() from anywhere.
    public static void TestAchievement(){
        Debug.Log("Triggering: " + test_achievementName);
        TriggerAchievement(test_achievementName);
    }
    
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
