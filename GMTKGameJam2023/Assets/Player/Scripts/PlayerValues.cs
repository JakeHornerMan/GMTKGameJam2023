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

    public static void SetDefaultValues()
    {
        Cars = null;
        ultimate = null;
        missedChickenLives = 3;
        carWalletNodes = 3;
        playerCash = 0;
    }

    public static void SetRound5Values()
    {
        Cars = null;
        ultimate = null;
        missedChickenLives = 3;
        carWalletNodes = 3;
        playerCash = 50;
    }

    public static void ResetCash()
    {
        playerCash = 0;
    }

    public static void LoadSaveData(GameSave data)
    {
        Cars = new List<Car>();
        foreach (Car car in data.Cars){
            Debug.Log("Jake - car: "+  car);
            Cars.Add(car);
        }
        ultimate = data.ultimate;
        missedChickenLives = data.missedChickenLives;
        carWalletNodes = data.carWalletNodes;
        startingEnergy = data.startingEnergy;
        playerCash = data.playerCash;
    }

    
}
