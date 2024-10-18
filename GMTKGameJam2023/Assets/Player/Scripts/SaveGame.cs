using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class SaveGame : MonoBehaviour
{
    static string hash = "123456@abc";

    public static GameSave saveDataLoaded = null;
    public static bool isLoadingASave = false;

    public static void SaveTheGame(){
        GameSave gameSave= new GameSave();
        string jsonData = JsonUtility.ToJson(gameSave);
        Debug.Log(jsonData);
        string saveData = Encrypts(jsonData);
        Debug.Log(saveData);

        string filePath = Application.persistentDataPath + "/SaveGameDoNotTouch.txt";
        System.IO.File.WriteAllText(filePath, saveData);
        Debug.Log("Save Game to: "+ filePath);
    }

    public static void LoadTheGame(){
        string filePath = Application.persistentDataPath + "/SaveGameDoNotTouch.txt";
        if(DoesSaveFileExist()){
            Debug.Log("File Exists: "+ filePath);
            string saveData = System.IO.File.ReadAllText(filePath);
            Debug.Log(saveData);
            string jsonData = Dncrypts(saveData);
            Debug.Log(jsonData);
            GameSave gameSave = JsonUtility.FromJson<GameSave>(jsonData);

            saveDataLoaded = gameSave;
            Debug.Log(saveDataLoaded.ToString());
        }
        else{
            Debug.Log("File Does Not Exist "+ filePath);
        }
    }

    public static bool DoesSaveFileExist(){
        string filePath = Application.persistentDataPath + "/SaveGameDoNotTouch.txt";
        if (File.Exists(@filePath)) {
            return true;
        }
        return false;
    }

    public static void SetGameDataForGame(){
        Debug.Log(saveDataLoaded.ToString());
        PlayerValues.LoadSaveData(saveDataLoaded);
        Points.LoadSaveData(saveDataLoaded);
        GameProgressionValues.LoadSaveData(saveDataLoaded);
    }

    public static void DeleteSaveFileAndStaticData(){
        string filePath = Application.persistentDataPath + "/SaveGameDoNotTouch.txt";
        if(DoesSaveFileExist()){
            File.Delete(@filePath);
            Debug.Log("File Deleted: "+ filePath);
        }
        
        saveDataLoaded = null;
    }
    
    public static string Encrypts(String input){
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using(TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] output = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(output, 0, output.Length);
            }
        }
    }

    public static string Dncrypts(String input){
        byte[] data = Convert.FromBase64String(input);
        using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] output = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(output);
            }
        }
    }
}

public class GameSave
{
    //PlayerValues
    public List<string> Cars;
    public Ultimate ultimate;
    public int missedChickenLives;
    public int carWalletNodes;
    public int startingEnergy;
    public int playerCash;

    // Points
    public int killCount;
    public int safelyCrossedChickens;
    public int playerScore;
    public int totalTokens;

    // GameProgressionValues
    public int RoundNumber;

    public GameSave(){
        //PlayerValues
        // foreach(string c in Cars){
        //     Cars.Add(c);
        // }
        // Cars = PlayerValues.Cars;
        Cars = new List<string>();
        foreach(var car in PlayerValues.Cars){
            string carName = car.GetComponent<ObjectInfo>().objectName;
            Cars.Add(carName);
        }
        ultimate = PlayerValues.ultimate;
        missedChickenLives = PlayerValues.missedChickenLives;
        carWalletNodes = PlayerValues.carWalletNodes;
        startingEnergy = PlayerValues.startingEnergy;
        playerCash = PlayerValues.playerCash;

        //Points
        killCount = Points.killCount;
        safelyCrossedChickens = Points.safelyCrossedChickens;
        playerScore = Points.playerScore;
        totalTokens = Points.totalTokens;

        // GameProgressionValues
        RoundNumber = GameProgressionValues.RoundNumber;
    }

    public void SetValues(){
        //PlayerValues
        // PlayerValues.Cars = Cars;
        PlayerValues.ultimate = ultimate;
        PlayerValues.missedChickenLives = missedChickenLives;
        PlayerValues.carWalletNodes = carWalletNodes;
        PlayerValues.startingEnergy = startingEnergy;
        PlayerValues.playerCash = playerCash;

        //Points
        Points.killCount = killCount;
        Points.safelyCrossedChickens = safelyCrossedChickens;
        Points.playerScore = playerScore;
        Points.totalTokens = totalTokens;

        // GameProgressionValues
        GameProgressionValues.RoundNumber = RoundNumber;
    }

    public string ToString(){

    // Join the list of Cars into a string representation
    string carsString = Cars != null ? "Cars: " + Cars.Count : "No Cars";
    
    return $"Cars: {carsString}\n" +
           $"Ultimate: {ultimate}\n" +
           $"Missed Chicken Lives: {missedChickenLives}\n" +
           $"Car Wallet Nodes: {carWalletNodes}\n" +
           $"Starting Energy: {startingEnergy}\n" +
           $"Player Cash: {playerCash}\n" +
           $"Kill Count: {killCount}\n" +
           $"Safely Crossed Chickens: {safelyCrossedChickens}\n" +
           $"Player Score: {playerScore}\n" +
           $"Total Tokens: {totalTokens}";
    }

}
