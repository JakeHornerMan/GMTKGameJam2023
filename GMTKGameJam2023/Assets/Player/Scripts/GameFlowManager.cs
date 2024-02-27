using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private SpecialChicken[] SpecialChickens;
    [SerializeField] private List<SpecialChicken> chickenPot;
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
                StandardChickenWaveCreate(i);
            }
        }
        GameProgressionValues.RoundNumber ++;
    }

    private void StandardChickenWaveCreate(int roundNumber){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = GameProgressionValues.standardRoundTime;
        newChickenWave.standardChickenAmounts = standardChickenAmount();
        if(roundNumber == 0){
            newChickenWave.wavePrompt = "Round: " + GameProgressionValues.RoundNumber;
        }
        else {
            newChickenWave.wavePrompt = "Chciken Amounts: " + newChickenWave.standardChickenAmounts;
        }
        newChickenWave.chickenIntesity = chickenIntesitySet();
        newChickenWave.coinAmount = RoundCoinSet();
        newChickenWave.specialChickens = SpecialChickenListSet();
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

    private List<SpecialChicken> SpecialChickenListSet(){
        List<SpecialChicken> specialChickensList = new List<SpecialChicken>();
        for (int i = 1; i <= GameProgressionValues.RoundNumber; i++){
            int getChickenAt = Random.Range(0, chickenPot.Count -1);
            SpecialChicken specialChicken = chickenPot[getChickenAt].DeepClone();
            // Debug.Log(specialChicken.chicken.name);

            if(GameProgressionValues.RoundNumber < GameProgressionValues.standardRoundTime){
                specialChicken.timeToSpawn = Random.Range(1,GameProgressionValues.standardRoundTime-1);
            }
            else{
                if(i < GameProgressionValues.standardRoundTime)
                    specialChicken.timeToSpawn = (float)i; 
                if(i > GameProgressionValues.standardRoundTime)
                    specialChicken.timeToSpawn = Random.Range(1,GameProgressionValues.standardRoundTime-1);
            }
            specialChickensList.Add(specialChicken);
        }

        return specialChickensList;
    } 

    private int RoundCoinSet(){
        if(GameProgressionValues.RoundNumber > 15){
            return 30;
        }
        return 15 + (GameProgressionValues.RoundNumber);
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

    public SpecialChicken() { }
    public SpecialChicken(SpecialChicken specialChicken) {
        timeToSpawn = specialChicken.timeToSpawn;
        chicken = specialChicken.chicken;
        topSpawn = specialChicken.topSpawn;
        bottomSpawn = specialChicken.bottomSpawn;
    }

    public SpecialChicken DeepClone(){
        SpecialChicken specialChicken = new SpecialChicken();
        specialChicken.timeToSpawn = timeToSpawn;
        specialChicken.chicken = chicken;
        specialChicken.topSpawn = topSpawn;
        specialChicken.bottomSpawn = bottomSpawn;

        return specialChicken;
    }

}

[System.Serializable]
public class RankingRequirement
{
    public int minScore = 0;
    public string rankingString = "Poultry Terrorizer";
}
