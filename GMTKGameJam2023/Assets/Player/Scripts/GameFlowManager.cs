using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private float emptyWaveTime;
    [SerializeField] private SpecialChicken[] SpecialChickens;
    [SerializeField] private List<SpecialChicken> chickenPot;
    [SerializeField] private List<SpecialChicken> potCandidates;
    [SerializeField] public ChickenWave[] bonusWaves;
    private GameManager gameManager;

    private int standardChickenAmount = 0;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void newRound(){
        setStandardAmount();
        SetSpecialChickenCandidates();

        for(int i =1; i <= 4; i++){
            if(i == 2){
                SpecialWave();
            }
            else if(i == 4){
                EmptyWave();
            }
            else{
                StandardChickenWaveCreate(i);
            }
        }
    }

    private void StandardChickenWaveCreate(int waveNumber){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = GameProgressionValues.standardRoundTime;
        newChickenWave.standardChickenAmounts = standardChickenAmount;

        if(waveNumber == 1){
            newChickenWave.wavePrompt = "Round: " + GameProgressionValues.RoundNumber;
        }
        else {
            newChickenWave.wavePrompt = "Chciken Amounts: " + newChickenWave.standardChickenAmounts;
            //TODO: insert funny prompts funtiality
        }
        newChickenWave.chickenIntesity = chickenIntesitySet();
        newChickenWave.coinAmount = RoundCoinSet();
        newChickenWave.specialChickens = SpecialChickenListSet();
        gameManager.waves.Add(newChickenWave);
        // Debug.Log(newChickenWave.wavePrompt);
    }

    // Old System
    // private int standardChickenAmount(){
        
    //     float amount = GameProgressionValues.standardChickenAmountForStart;
    //     if(GameProgressionValues.RoundNumber <= 30){
    //         for(int i = 0; i < GameProgressionValues.RoundNumber; i ++)
    //         {
    //             amount = amount * GameProgressionValues.standardChickenAmountMultiplier;
    //         }
    //     }
    //     else{
    //         int rando = Random.Range(15,30);
    //         for(int i = 0; i < rando; i ++)
    //         {
    //             amount = amount * GameProgressionValues.standardChickenAmountMultiplier;
    //         }
    //     }
    //     return (int)amount;

    //     new System
    // }

    private void setStandardAmount(){
        if(GameProgressionValues.RoundNumber > 25){
            standardChickenAmount  = Random.Range(330, 600);
        }
        else{
            standardChickenAmount = GameProgressionValues.standardChickenAmountForStart;
            for(int i = 2; i <= GameProgressionValues.RoundNumber; i++){
                int multiplier = (int)Mathf.Floor((float)i/5f);
                // Debug.Log("multi: " + multiplier);
                standardChickenAmount += 10 + (5 * multiplier);
            }
        } 
    }

    private void SetSpecialChickenCandidates()
    {
         //find amount of point for the round
        int chickenPoints = GameProgressionValues.RoundNumber*2;
        //new list to store potential chickens theat we can use
        foreach (SpecialChicken chic in chickenPot){
            if(chic.difficultyRank <= chickenPoints){
                potCandidates.Add(chic.DeepClone());
            }
        }
    }

    private List<SpecialChicken> SpecialChickenListSet(){
        List<SpecialChicken> specialChickensList = new List<SpecialChicken>();
        // for (int i = 1; i <= GameProgressionValues.RoundNumber*2; i++){
        //     int getChickenAt = Random.Range(0, chickenPot.Count);
        //     SpecialChicken specialChicken = chickenPot[getChickenAt].DeepClone();
        //     // Debug.Log(specialChicken.chicken.name);

        //     if(GameProgressionValues.RoundNumber < GameProgressionValues.standardRoundTime){
        //         specialChicken.timeToSpawn = Random.Range(1,GameProgressionValues.standardRoundTime-1);
        //     }
        //     else{
        //         if(i < GameProgressionValues.standardRoundTime)
        //             specialChicken.timeToSpawn = (float)i; 
        //         if(i > GameProgressionValues.standardRoundTime)
        //             specialChicken.timeToSpawn = Random.Range(1,GameProgressionValues.standardRoundTime-1);
        //     }
        //     specialChickensList.Add(specialChicken);
        // }

        //find amount of point for the wave
        int chickenPoints = GameProgressionValues.RoundNumber*2;
        //new list to store potential chickens theat we can use
        // List<SpecialChicken> potCandidates = new List<SpecialChicken>();
        // foreach (SpecialChicken chic in chickenPot){
        //     if(chic.difficultyRank <= chickenPoints)
        //         potCandidates.Add(chic);
        // }

        //find random chickens in candidates for our wave
        do{
            int getChickenAt = Random.Range(0, potCandidates.Count);
            SpecialChicken specialChicken = potCandidates[getChickenAt].DeepClone();
            if(specialChicken.difficultyRank <= chickenPoints){
                specialChicken.timeToSpawn = Random.Range(1,GameProgressionValues.standardRoundTime-1);
                chickenPoints = chickenPoints - specialChicken.difficultyRank;
                specialChickensList.Add(specialChicken);
            }
        }
        while(chickenPoints > 0);

        return specialChickensList;
    } 

    private int RoundCoinSet(){
        if(GameProgressionValues.RoundNumber > 19){
            return 50;
        }
        return 30 + (GameProgressionValues.RoundNumber);
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
        copyWave.coinAmount = 20;
        gameManager.waves.Add(copyWave);
        // Debug.Log(copyWave.wavePrompt);
    }

    private void EmptyWave(){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = emptyWaveTime;
        newChickenWave.standardChickenAmounts = 0;
        newChickenWave.wavePrompt = "Times Nearly Up!";
        newChickenWave.waveSound = "LastSeconds";
        newChickenWave.chickenIntesity = 0;
        newChickenWave.coinAmount = 0;
        newChickenWave.specialChickens = null;
        gameManager.waves.Add(newChickenWave);
        // Debug.Log(newChickenWave.wavePrompt);
    }
}

[System.Serializable]
public class ChickenWave
{
    public float roundTime;
    public string wavePrompt;
    public string waveSound;
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
    public int difficultyRank;
    public bool topSpawn;
    public bool bottomSpawn;

    public SpecialChicken() { }
    public SpecialChicken(SpecialChicken specialChicken) {
        timeToSpawn = specialChicken.timeToSpawn;
        chicken = specialChicken.chicken;
        topSpawn = specialChicken.topSpawn;
        bottomSpawn = specialChicken.bottomSpawn;
        difficultyRank = specialChicken.difficultyRank;
    }

    public SpecialChicken DeepClone(){
        SpecialChicken specialChicken = new SpecialChicken();
        specialChicken.timeToSpawn = timeToSpawn;
        specialChicken.chicken = chicken;
        specialChicken.topSpawn = topSpawn;
        specialChicken.bottomSpawn = bottomSpawn;
        specialChicken.difficultyRank = difficultyRank;

        return specialChicken;
    }

}

[System.Serializable]
public class RankingRequirement
{
    public int minScore = 0;
    public string rankingString = "Poultry Terrorizer";
}
