using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgressionValues
{
    public static int sceneIndex = 0; //stores scene index
    public static int RoundNumber = 1; //default : 1
    public static float standardRoundTime = 3f; //default : 30f
    public static int standardChickenAmountForStart = 20; //default : 30
    public static float standardChickenAmountMultiplier = 1.1f; //default : 1.1f

    public static void SetDefaultValues(){
        RoundNumber = 1;
        standardRoundTime = 30f;
        standardChickenAmountForStart = 30;
        standardChickenAmountMultiplier = 1.1f;
    }
}
