using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] public List<SpecialChicken> chickenPot;
    [SerializeField] public ChickenWave[] bonusWaves;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void newRound(){
        // RoundProgression();
        int randomSpecialWave = Random.Range(1,2);
        for(int i =0; i < 4; i++){
            if(i == randomSpecialWave){
                SpecialWave();
            }
            else{
                StandardChickenWaveCreate();
            }
        }
        GameProgressionValues.RoundNumber ++;
    }

    private void StandardChickenWaveCreate(){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = GameProgressionValues.standardRoundTime;
        newChickenWave.wavePrompt = "Lets Go!";
        newChickenWave.standardChickenAmounts = standardChickenAmount();
        // newChickenWave.standardChickenAmounts = (int)(GameProgressionValues.standardChickenAmountForLevel);
        newChickenWave.wavePrompt = "Chciken Amounts: " + newChickenWave.standardChickenAmounts;
        newChickenWave.chickenIntesity = chickenIntesitySet();
        newChickenWave.coinAmount = RoundCoinSet();
        newChickenWave.specialChickens = null;
        gameManager.waves.Add(newChickenWave);
    }

    private int standardChickenAmount(){
        // GameProgressionValues.standardChickenAmountForLevel = GameProgressionValues.standardChickenAmountForLevel * GameProgressionValues.standardChickenAmountMultiplier;
        float amount = GameProgressionValues.standardChickenAmountForStart;
        if(GameProgressionValues.RoundNumber <= 30){
            for(int i = 0; i < GameProgressionValues.RoundNumber; i ++)
            {
                amount = amount * GameProgressionValues.standardChickenAmountMultiplier;
            }
        }
        else{
            int rando = Random.Range(15,30);
            for(int i = 0; i < rando; i ++)
            {
                amount = amount * GameProgressionValues.standardChickenAmountMultiplier;
            }
        }
        return (int)amount;
    }

    private void SpecialChickenListSet(ChickenWave newChickenWave){
        //need to add special chicken progression here
    } 

    private int RoundCoinSet(){
        //need to make this smarter
        return 15;
    }

    private int chickenIntesitySet(){
        int intensity = 1;
        if(GameProgressionValues.RoundNumber <= 8){
            intensity = Random.Range(1,3);
        }
        if(GameProgressionValues.RoundNumber <= 16){
            intensity = Random.Range(2,4);
        }
        if(GameProgressionValues.RoundNumber <= 24){
            intensity = Random.Range(3,5);
        }
        if(GameProgressionValues.RoundNumber <= 32){
            intensity = Random.Range(4,5);
        }
        if(GameProgressionValues.RoundNumber > 32){
            intensity = 5;
        }
        return intensity;
    }

    private void SpecialWave(){
        int getWave = Random.Range(0,bonusWaves.Length);
        ChickenWave copyWave = bonusWaves[getWave].DeepClone();
        gameManager.waves.Add(copyWave);
    }
}

[System.Serializable]
public class ChickenWave
{
    public float roundTime;
    public string wavePrompt;
    public int standardChickenAmounts;
    public int chickenIntesity = 0;
    public int coinAmount  = 0;
    public List<SpecialChicken> specialChickens;
    public ChickenWave() { }
    public ChickenWave(ChickenWave chickenWave) {
        roundTime = chickenWave.roundTime;
        wavePrompt = chickenWave.wavePrompt;
        standardChickenAmounts = chickenWave.standardChickenAmounts;
        chickenIntesity = chickenWave.chickenIntesity;
        coinAmount = chickenWave.coinAmount;
        specialChickens = new List<SpecialChicken>();
    }
    public ChickenWave DeepClone(){
        ChickenWave chickenWave = new ChickenWave();
        chickenWave = (ChickenWave)this.MemberwiseClone();
        chickenWave.specialChickens = new List<SpecialChicken>(specialChickens);

        return chickenWave;
    }
}

[System.Serializable]
public class SpecialChicken
{
    public float timeToSpawn;
    public GameObject chicken;
    public bool topSpawn;
    public bool bottomSpawn;

}

[System.Serializable]
public class RankingRequirement
{
    public int minScore = 0;
    public string rankingString = "Poultry Terrorizer";
}
