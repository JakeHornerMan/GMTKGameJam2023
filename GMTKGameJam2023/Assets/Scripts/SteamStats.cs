using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamStats : MonoBehaviour
{
    private const string chickenKills_statName = "CHICKEN_KILLS";
    private const string totalCars_statName = "TOTAL_CARS";
    private const string scoreVehicle_statName = "SCORE_VEHICLE";
    private const string topRound_statName = "TOP_ROUND";

    public static StatResultValue GetChickenKills(){
        int score = GetStat(chickenKills_statName);
        return new StatResultValue("Total chicken killed", score);
    }

    public static StatResultValue GetTotalCars(){
        int score = GetStat(totalCars_statName);
        return new StatResultValue("Total cars placed", score);
    }

    public static StatResultValue GetHighestScoreVehicle(){
        int score = GetStat(scoreVehicle_statName);
        return new StatResultValue("Highest round reached", score);
    }

    public static StatResultValue GetTopRound(){
        int score = GetStat(topRound_statName);
        return new StatResultValue("Highest score from a vehicle", score);
    }

    public static void SetChickenKills(int score){
        SetStat(chickenKills_statName, score);
    }

    public static void SetTotalCars(int score){
        SetStat(totalCars_statName, score);
    }

    public static void SetHighestScoreVehicle(int score){
        SetStat(scoreVehicle_statName, score);
    }

    public static void SetTopRound(int score){
        SetStat(topRound_statName, score);
    }

    private static int GetStat(string statName = "TEST_STAT"){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat(statName, out int count);
            Debug.Log(statName + " Stat Count: " + count);
            SteamUserStats.StoreStats();
            return count;
        }
        else{
            Debug.Log("Steam Manager not Initialised");
            return 0;
        }
    }

    private static void SetStat(string statName = "TEST_STAT", int score = 0){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat(statName, out int count);
            Debug.Log(statName + " Before Count: " + count);
            count += score;
            
            Steamworks.SteamUserStats.SetStat(statName, count);
            Debug.Log(statName + " After Count: " + count);
            SteamUserStats.StoreStats();
        }
        else{
            Debug.Log("Steam Manager not Initialised");
        }
    }
}

public class StatResultValue
{
    public string Title;
    public int Score;

    public StatResultValue(string title, int score)
    {
        Title = title;
        Score = score;
    }

    public void Log(){
        Debug.Log($"Stat Name: {Title}, Score: {Score}");
    }
}