using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgressionValues
{
    public static int sceneIndex = 0; //stores scene index
    public static int RoundNumber = 20; //default : 1
    public static float standardRoundTime = 15f; //default : 30f
    public static int standardChickenAmountForStart = 30; //default : 30
    public static float standardChickenAmountMultiplier = 1.1f; //default : 1.1f
    public static List<GameObject> LaneMap = new List<GameObject>();
    public static void SetDefaultValues()
    {
        RoundNumber = 1;
        standardRoundTime = 30f;
        standardChickenAmountForStart = 30;
        standardChickenAmountMultiplier = 1.1f;
    }

    public static void SetRound5Values()
    {
        Debug.Log("ROUND 5 SET HERE!!!!! GAME PROGRESSION");
        RoundNumber = 5;
        standardRoundTime = 30f;
        standardChickenAmountForStart = 30;
        standardChickenAmountMultiplier = 1.1f;
    }

    public static void SetLaneMap(List<GameObject> lanes)
    {
        // List<GameObject> LaneMap = new List<GameObject>();
        LaneMap = lanes;
    }
}
