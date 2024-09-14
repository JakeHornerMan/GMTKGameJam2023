using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Points
{
    public static int killCount = 0;
    public static int safelyCrossedChickens = 0;
    public static int playerScore = 0;
    public static int totalTokens = 0;

    public static void SetDefaultValues(){
        killCount = 0;
        safelyCrossedChickens =0;
        playerScore = 0;
        totalTokens = 0;
    }

    public static void LoadSaveData(GameSave data)
    {
        killCount = data.killCount;
        safelyCrossedChickens = data.safelyCrossedChickens;
        playerScore = data.playerScore;
        totalTokens = data.totalTokens;
    }

}