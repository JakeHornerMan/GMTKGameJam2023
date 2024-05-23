using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerValues
{
    public static List<Car> Cars { get; set; }
    public static Ultimate ultimate;
    public static int missedChickenLives = 3;
    public static int carWalletNodes = 3;
    public static int startingEnergy = 0;
    public static int playerCash = 0;

    public static void SetDefaultValues(){
        Cars.Clear();
        ultimate = null;
        missedChickenLives = 3;
        carWalletNodes = 3;
        playerCash = 0;
    }
}
