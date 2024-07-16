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
    private const string topCombo_statName = "TOP_COMBO";
    private const string totalUlt_statName = "TOTAL_ULT";
    private const string totalMissedChickens_statName = "MISSED_CHICKENS";

    public static StatResultValue GetChickenKills(){
        int score = GetStat(chickenKills_statName);
        StatResultValue stat = new StatResultValue("Total chicken stopped", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static StatResultValue GetTotalCars(){
        int score = GetStat(totalCars_statName);
        StatResultValue stat = new StatResultValue("Total cars placed", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static StatResultValue GetHighestScoreVehicle(){
        int score = GetStat(scoreVehicle_statName);
        StatResultValue stat = new StatResultValue("Highest score from a vehicle", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static StatResultValue GetTopRound(){
        int score = GetStat(topRound_statName);
        StatResultValue stat = new StatResultValue("Highest round reached", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static StatResultValue GetTopCombo(){
        int score = GetStat(topCombo_statName);
        StatResultValue stat = new StatResultValue("Highest vehicle combo", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static StatResultValue GetTotalUltimates(){
        int score = GetStat(totalUlt_statName);
        StatResultValue stat = new StatResultValue("Total ultimates placed", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static StatResultValue GetTotalMissedChickens(){
        int score = GetStat(totalMissedChickens_statName);
        StatResultValue stat = new StatResultValue("Chickens that crossed the road", score);
        Debug.Log(stat.ToString());
        return stat;
    }

    public static void SetChickenKills(int score){
        SetAddStat(chickenKills_statName, score);
    }

    public static void SetTotalCars(int score){
        SetAddStat(totalCars_statName, score);
    }

    public static void SetHighestScoreVehicle(int score){
        SetBestStat(scoreVehicle_statName, score);
    }

    public static void SetTopRound(int score){
        SetBestStat(topRound_statName, score);
    }

    public static void SetTopCombo(int score){
        SetBestStat(topCombo_statName, score);
    }

    public static void SetTotalUltimates(int score){
        SetAddStat(totalUlt_statName, score);
    }

    public static void SetTotalMissedChickens(int score){
        SetAddStat(totalMissedChickens_statName, score);
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

    private static void SetAddStat(string statName = "TEST_STAT", int score = 0){
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

    private static void SetBestStat(string statName = "TEST_STAT", int score = 0){
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetStat(statName, out int count);
            Debug.Log(statName + " Before: " + count);
            if(count < score){
                Steamworks.SteamUserStats.SetStat(statName, score);
                Debug.Log(statName + " After : " + score);
                SteamUserStats.StoreStats();
            }
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