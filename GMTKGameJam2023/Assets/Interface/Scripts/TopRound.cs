using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TopRound
{
    public static int topRound =0;

    public static void LoadRound(){
        string filePath = Application.persistentDataPath + "/RoundData.json";
        try{
            string jsonData = System.IO.File.ReadAllText(filePath);

            RoundData roundData = JsonUtility.FromJson<RoundData>(jsonData);

            topRound = roundData.TopRound;

            Debug.Log("Round file path: "+ filePath);
        }
        catch(System.Exception e){
            Debug.Log("Error finding file: "+ filePath);
            topRound = 0;
        }
        Debug.Log("Round to: " + topRound);
    }

    public static void SaveRound(){
        RoundData roundData = new RoundData(GameProgressionValues.RoundNumber);
        string jsonData = JsonUtility.ToJson(roundData);
        string filePath = Application.persistentDataPath + "/RoundData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Round to: "+ filePath);
    }
}

[System.Serializable]
public class RoundData
{
    public int TopRound;

    public RoundData(int topRound){
        TopRound = topRound;
    }
}
